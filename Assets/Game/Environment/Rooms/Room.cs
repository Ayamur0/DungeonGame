using Assets.Game.Environment.Rooms;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Spawn,
    Exit,
    Shop,
    Battle,
    Explore,
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
    public bool Visited = false;
    public LevelManager LevelManager;
    public List<GameObject> NeighborRooms = new List<GameObject>();

    public RoomContent Content;
    private float gateSize = 1.5f;

    public List<Vector3> GetRandomSpawns(int amount)
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        if(Type == RoomType.Battle && this.Content)
        {
            if (this.Content.EnemySpawns.Count > 0)
            {
                for(int i = 0; i < amount; i++)
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
    }

    public void OpenGates()
    {
        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            gate.transform.position = new Vector3(gatePos.x, -gateSize, gatePos.z);
        }
    }

    public void CloseGates()
    {
        if (Visited)
            return;

        foreach (var gate in Gates)
        {
            var gatePos = gate.transform.position;
            gate.transform.position = new Vector3(gatePos.x, 1, gatePos.z);
        }
    }

    public void OnPlayerEntered()
    {
        this.PlayerInRoom = true;
        this.LevelManager.SetActiveRoom(this.gameObject);
        CloseGates();
    }

    public void OnPlayerExit()
    {
        this.PlayerInRoom = false;
        OpenGates();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == PlayerTag)
        {
            OnPlayerExit();
        }
    }
}