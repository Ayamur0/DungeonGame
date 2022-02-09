using System.Collections;
using Assets.Game.Environment.Rooms;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomType
{
    Spawn,
    Exit,
    Shop,
    Battle,
    Explore,
}

public enum NeighborRoomPosition
{
    Up,
    Left,
    Right,
    Down,
}

public class Room : MonoBehaviour
{
    [Header("Settings")]
    public int Size = 20;

    public RoomType Type = RoomType.Battle;
    public CellNeighborInfo NeighborInfos;
    public string PlayerTag = "Player";
    public List<GameObject> Gates = new List<GameObject>();

    public Vector2 CellPosition = Vector2.zero;
    public bool PlayerInRoom { get; private set; }
    public bool GatesClosed { get; private set; }
    public bool Visited = false;
    public LevelManager LevelManager;
    public Dictionary<NeighborRoomPosition, GameObject> NeighborRooms;
    public AudioClip DoorsMoving;

    public GameObject FogOfWarPlane;
    public RoomContent Content;
    public bool KeepDoorsClosed = false;

    private float gateSize = 1.5f;
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private SoundManager soundManager;
    private bool isDirty;

    public List<Vector3> GetRandomSpawns(int amount)
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        if (Type == RoomType.Battle && this.Content)
        {
            if (this.Content.EnemySpawns.Count > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    var randomIndex = Random.Range(0, this.Content.EnemySpawns.Count - 1);
                    var maxRange = this.Content.EnemySpawns[randomIndex].Range;
                    var randomRange = Random.Range(0f, maxRange);

                    float angle = (float)(2.0 * Mathf.PI * Random.Range(0f, 1f));
                    var center = this.Content.EnemySpawns[randomIndex].transform.position;
                    var x = center.x + randomRange * Mathf.Cos(angle);
                    var y = center.y + randomRange * Mathf.Sin(angle);

                    spawnPositions.Add(new Vector3(x, y));
                }
            }
        }

        return spawnPositions;
    }

    // Start is called before the first frame update
    private void Start()
    {
        OpenGates();
        this.Content = GetComponentInChildren<RoomContent>();

        if (this.Content)
        {
            var portal = this.Content.GetComponentInChildren<Portal>();
            if (portal)
            {
                portal.LevelManager = LevelManager;
            }
        }

        if(this.FogOfWarPlane)
        {
            this.FogOfWarPlane.SetActive(this.Type != RoomType.Spawn);
        }

        this.soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    public void Update()
    {
        CheckEnemiesStatus();
    }

    private void CheckEnemiesStatus()
    {
        if (this.spawnedEnemies.Count > 0)
        {
            var waveCleared = this.spawnedEnemies.All(x => x == null);
            if (waveCleared)
            {
                this.spawnedEnemies.Clear();
                RoomCleared();
            }
        }
    }

    public void RoomCleared()
    {
        this.Visited = true;
        this.OpenGates();

        this.soundManager?.PlayMainMusic();
    }

    public void OpenGates()
    {
        if(KeepDoorsClosed)
            return;

        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            StartCoroutine(TweenPosition(gate, new Vector3(gatePos.x, -gateSize, gatePos.z), 2f));
        }
        GatesClosed = false;
    }

    public void CloseGates()
    {
        if (Visited || Type != RoomType.Battle)
            return;
        
        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            StartCoroutine(TweenPosition(gate, new Vector3(gatePos.x, 1f, gatePos.z), 2f));
        }

        GatesClosed = true;
    }

    public void OnPlayerEntered(GameObject player)
    {
        this.PlayerInRoom = true;
        this.LevelManager.SetActiveRoom(this.gameObject);
        CloseGates();

        if (this.Type == RoomType.Battle)
        {
            var cameraController = GameObject.FindObjectOfType<CameraController>();
            cameraController.SetEnemyZoom();
        }

        if (this.FogOfWarPlane)
        {
            this.FogOfWarPlane.SetActive(false);
        }

        if (this.Content && !this.Visited)
        {
            player.GetComponent<PlayerAPI>().DecreaseActiveItemCooldown();
            var enemyGenerator = this.Content.GetComponent<EnemyGenerator>();
            int amountSpawn = 5;
            if (enemyGenerator != null)
            {
                if (LevelManager != null)
                {
                    int x = LevelManager.CurrentStage;
                    
                    if (x <= 1)
                    {
                        amountSpawn = 3;
                    }
                    else if (2 <= x && x <= 5)
                    {
                        amountSpawn = 5;
                    }
                    else if (6 <= x && x <= 8)
                    {
                        amountSpawn = 7;
                    }
                    else
                    {
                        amountSpawn = 10;
                    }
                }
                var spawns = this.Content.GetRandomSpawns(amountSpawn);
                this.spawnedEnemies = enemyGenerator.GenerateEnemies(spawns, this);
            }

            if (this.Type == RoomType.Battle)
            {
                this.soundManager?.PlayBattleMusic();
            }
        }
    }

    public void OnPlayerExit()
    {
        this.PlayerInRoom = false;
        OpenGates();

        if (this.Type == RoomType.Battle)
        {
            var cameraController = GameObject.FindObjectOfType<CameraController>();
            cameraController.SetDefaultZoom();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerEntered(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerExit();
        }
    }

    IEnumerator TweenPosition(GameObject target, Vector3 targetPosition, float duration)
    {
        this.isDirty = true;
        Vector3 previousPosition = target.transform.position;
        float time = 0.0f;
        do
        {
            time += Time.deltaTime;
            target.transform.position = Vector3.Lerp(previousPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, time / duration));
            yield return 0;
        } while (time < duration);
        this.isDirty = false;
    }
}