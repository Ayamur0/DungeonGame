using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool HideRooms = true;

    public int CurrentStage = 1;

    private GameObject activeRoom;
    private LevelGenerator levelGenerator;
    private Dictionary<NeighborRoomPosition, GameObject> activeRooms;

    // Start is called before the first frame update
    private void Start()
    {
        this.levelGenerator = this.GetComponent<LevelGenerator>();
    }

    public void LoadNextState()
    {
        if (this.levelGenerator)
        {
            var lvlSettings = this.levelGenerator.Settings;
            lvlSettings.Rooms += 5;
            this.levelGenerator.GenerateLevel(lvlSettings);

            this.CurrentStage++;

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                // player spawns aloways in the center of the world
                player.transform.position = new Vector3(0, 0, 0);
            }
        }
    }

    public void SetActiveRoom(GameObject newRoom)
    {
        if (this.activeRoom && HideRooms)
        {
            if (this.activeRooms.Count > 0)
            {
                foreach (var room in activeRooms.Values)
                {
                    room.SetActive(false);
                }
            }
        }

        this.activeRoom = newRoom;
        this.activeRoom.SetActive(true);
        EnableNeighborRooms();
    }

    public void EnableNeighborRooms()
    {
        if (this.activeRoom)
        {
            Dictionary<NeighborRoomPosition, GameObject> rooms = GetNeighborRooms(this.activeRoom);
            foreach (var roomObj in rooms.Values)
            {
                var room = roomObj.gameObject.GetComponent<Room>();
                room.NeighborRooms = rooms;
                roomObj.SetActive(true);
            }
            this.activeRooms = rooms;
        }
    }

    public Dictionary<NeighborRoomPosition, GameObject> GetNeighborRooms(GameObject activeRoom)
    {
        Dictionary<NeighborRoomPosition, GameObject> rooms = new Dictionary<NeighborRoomPosition, GameObject>();
        var room = activeRoom.GetComponent<Room>();

        var pos = room.CellPosition;

        // right
        if (pos.x + 1 < this.levelGenerator.GetRooms().Length)
        {
            var rightRoom = this.levelGenerator.GetRooms()[(int)pos.x + 1, (int)pos.y];
            if (rightRoom)
                rooms.Add(NeighborRoomPosition.Right, rightRoom);
        }

        // left
        if (pos.x - 1 > 0)
        {
            var leftRoom = this.levelGenerator.GetRooms()[(int)pos.x - 1, (int)pos.y];
            if (leftRoom)
                rooms.Add(NeighborRoomPosition.Left, leftRoom);
        }

        // Up
        if (pos.y - 1 > 0)
        {
            var upRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y - 1];
            if (upRoom)
                rooms.Add(NeighborRoomPosition.Up, upRoom);
        }

        // Down
        if (pos.y + 1 < this.levelGenerator.GetRooms().Length)
        {
            var downRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y + 1];
            if (downRoom)
                rooms.Add(NeighborRoomPosition.Down, downRoom);
        }

        return rooms;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var room = this.activeRoom.GetComponent<Room>();
            if (room.Type == RoomType.Battle)
            {
                room.RoomCleared();
            }
        }
    }
}