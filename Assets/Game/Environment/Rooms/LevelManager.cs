using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool HideRooms = true;

    private GameObject activeRoom;
    private LevelGenerator levelGenerator;
    private List<GameObject> activeRooms;

    // Start is called before the first frame update
    private void Start()
    {
        this.levelGenerator = this.GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.activeRoom)
        {
        }
    }

    public void SetActiveRoom(GameObject newRoom)
    {
        if (this.activeRoom && !HideRooms)
        {
            if (this.activeRooms.Count > 0)
            {
                foreach (var room in activeRooms)
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
            var rooms = GetNeighborRooms(this.activeRoom);
            foreach (var room in rooms)
            {
                room.gameObject.GetComponent<Room>().NeighborRooms = rooms;
                room.SetActive(true);
            }
            this.activeRooms = rooms;
        }
    }

    public List<GameObject> GetNeighborRooms(GameObject activeRoom)
    {
        List<GameObject> rooms = new List<GameObject>();
        var room = activeRoom.GetComponent<Room>();

        var pos = room.CellPosition;

        // right
        if (pos.x + 1 < this.levelGenerator.GetRooms().Length)
        {
            var rightRoom = this.levelGenerator.GetRooms()[(int)pos.x + 1, (int)pos.y];
            if (rightRoom)
                rooms.Add(rightRoom);
        }

        // left
        if (pos.x - 1 > 0)
        {
            var leftRoom = this.levelGenerator.GetRooms()[(int)pos.x - 1, (int)pos.y];
            if (leftRoom)
                rooms.Add(leftRoom);
        }

        // Up
        if (pos.y - 1 > 0)
        {
            var upRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y - 1];
            if (upRoom)
                rooms.Add(upRoom);
        }

        // Down
        if (pos.y + 1 < this.levelGenerator.GetRooms().Length)
        {
            var downRoom = this.levelGenerator.GetRooms()[(int)pos.x, (int)pos.y + 1];
            if (downRoom)
                rooms.Add(downRoom);
        }

        return rooms;
    }
}