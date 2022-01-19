using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public bool HideRooms = true;

    public int CurrentStage = 1;

    public GameObject SpawnVFX;
    public AudioClip SpawnSfx;

    private GameObject activeRoom;
    private LevelGenerator levelGenerator;
    private Dictionary<NeighborRoomPosition, GameObject> activeRooms;
    private TransitionManager transitionManager;
    private AudioSource audioSource;

    public Action ActiveRoomChanged;
    public Action NextStageLoaded;

    public GameObject GetActiveRoom() {
        return this.activeRoom;
    }

    // Start is called before the first frame update
    private void Start() {
        this.levelGenerator = this.GetComponent<LevelGenerator>();
        this.transitionManager = FindObjectOfType<TransitionManager>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void LoadNextState() {
        if (this.transitionManager != null) {
            StartCoroutine(this.transitionManager.FadeToTransparent(2f, delegate (int i) { }));
        }

        if (this.levelGenerator) {
            FindObjectOfType<GameManager>().CurrentScore.CurrentStage = CurrentStage;

            var lvlSettings = this.levelGenerator.Settings;
            lvlSettings.Rooms += 1;

            if (lvlSettings.Rooms % 3 == 0) {
                lvlSettings.MaxShops++;
                lvlSettings.MaxExplore++;
            }


            this.levelGenerator.GenerateLevel(lvlSettings);
            this.CurrentStage++;

            // convert soulhearts to red hearts
            var playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null) {
                playerStats.ApplySoulHearts();
            }

            // remove all unused powerups
            var powerups = GameObject.FindGameObjectsWithTag("Powerup");
            if (powerups != null) {
                foreach (var powerup in powerups) {
                    if (powerup.GetComponent<SpriteRenderer>().enabled)
                        Destroy(powerup);
                }
            }

            // remove enemies
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies != null) {
                foreach (var enemy in enemies) {
                    Destroy(enemy);
                }
            }

            if (this.audioSource && this.SpawnSfx) {
                this.audioSource.clip = SpawnSfx;
                this.audioSource.Play();
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player) {
                // player spawns aloways in the center of the world
                player.transform.position = new Vector3(0, 0, 0);
            }

            if (SpawnVFX) {
                var spawnvfx = Instantiate(SpawnVFX);
                spawnvfx.transform.position = new Vector3(0, 0.25f, 0);
                spawnvfx.transform.localScale = new Vector3(3, 3, 3);
            }
        }

        NextStageLoaded?.Invoke();
    }

    public void SetActiveRoom(GameObject newRoom) {
        if (this.activeRoom && HideRooms) {
            if (this.activeRooms.Count > 0) {
                foreach (var room in activeRooms.Values) {
                    room.SetActive(false);
                }
            }
        }

        this.activeRoom = newRoom;
        this.activeRoom.SetActive(true);
        EnableNeighborRooms();

        this.ActiveRoomChanged?.Invoke();
    }

    public void EnableNeighborRooms() {
        if (this.activeRoom) {
            Dictionary<NeighborRoomPosition, GameObject> rooms = GetNeighborRooms(this.activeRoom);
            foreach (var roomObj in rooms.Values) {
                var room = roomObj.gameObject.GetComponent<Room>();
                room.NeighborRooms = rooms;
                roomObj.SetActive(true);
            }
            this.activeRooms = rooms;
        }
    }

    public Dictionary<NeighborRoomPosition, GameObject> GetNeighborRooms(GameObject activeRoom) {
        Dictionary<NeighborRoomPosition, GameObject> rooms = new Dictionary<NeighborRoomPosition, GameObject>();
        var room = activeRoom.GetComponent<Room>();

        var pos = room.CellPosition;

        // right
        if (pos.x + 1 < this.levelGenerator.GetRooms().Length) {
            var rightRoom = this.levelGenerator.GetRooms()[(int)pos.x + 1, (int)pos.y];
            if (rightRoom)
                rooms.Add(NeighborRoomPosition.Right, rightRoom);
        }

        // left
        if (pos.x - 1 > 0) {
            var leftRoom = this.levelGenerator.GetRooms()[(int)pos.x - 1, (int)pos.y];
            if (leftRoom)
                rooms.Add(NeighborRoomPosition.Left, leftRoom);
        }

        // Up
        if (pos.y - 1 > 0) {
            var upRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y - 1];
            if (upRoom)
                rooms.Add(NeighborRoomPosition.Up, upRoom);
        }

        // Down
        if (pos.y + 1 < this.levelGenerator.GetRooms().Length) {
            var downRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y + 1];
            if (downRoom)
                rooms.Add(NeighborRoomPosition.Down, downRoom);
        }

        return rooms;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            var room = this.activeRoom.GetComponent<Room>();
            if (room.Type == RoomType.Battle) {
                room.RoomCleared();
            }
        }
    }
}