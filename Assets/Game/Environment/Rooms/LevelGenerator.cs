using Assets.Game.Environment.Rooms;
using UnityEngine;
using Cell = Assets.Game.Environment.Rooms.Maze.Cell;

public class LevelGenerator : MonoBehaviour
{
    public int MapWidth;
    public int MapHeight;
    public int Rooms;
    public GameObject FourGateRoomPrefab;
    public GameObject TwoGateRoomPrefab;
    public GameObject OneGateRoomPrefab;
    public GameObject TwoGateLRoomPrefab;
    public GameObject ThreeGateRoomPrefab;

    private Maze maze;
    private GameObject[,] rooms;

    private void Start()
    {
        this.maze = new Maze(this.MapWidth, this.MapHeight, this.Rooms);
        this.rooms = new GameObject[MapWidth, MapHeight];

        ShowRooms();
    }

    /// <summary>
    /// Randomized Prim
    /// </summary>
    private void ShowRooms()
    {
        var cells = this.maze.Generate();
        SlowShow(cells);
    }

    /// <summary>
    ///  Delay display
    /// </summary>
    /// <returns></returns>
    private void SlowShow(Cell[] cells)
    {
        GameObject roomsContainer = new GameObject("Rooms");

        float roomSize = 42f;

        for (int i = 0; i < cells.Length; i++)
        {
            var neighborInfo = cells[i].NeighborInfo;
            var roomObject = CreateRoomObject(roomsContainer, neighborInfo);

            var room = roomObject.GetComponent<Room>();
            room.CellPosition = new Vector2(cells[i].X, cells[i].Y);
            rooms[cells[i].X, cells[i].Y] = roomObject;
            rooms[cells[i].X, cells[i].Y].transform.position = new Vector3(cells[i].X * room.Size, 0, cells[i].Y * room.Size);
            roomSize = room.Size;

            // start room
            if (i == 0)
            {
                roomObject.name = "Spawn";
                room.Type = RoomType.Spawn;
            }
            else if (i == cells.Length - 1)
            {
                roomObject.name = "Exit";
                room.Type = RoomType.Exit;
            }
            else
            {
                // set random object type
            }
        }

        foreach (var roomObject in rooms)
        {
            if (roomObject)
            {
                var room = roomObject.GetComponent<Room>();
                room.NeighborInfos = maze.GetCell((int)room.CellPosition.x, (int)room.CellPosition.y).NeighborInfo;
            }
        }

        // center room container
        var containerPos = roomsContainer.transform.position;
        var pos = new Vector3(containerPos.x - roomSize * MapWidth / 2, 0, containerPos.y - roomSize * MapHeight / 2);
        roomsContainer.transform.position = pos;
    }

    private GameObject CreateRoomObject(GameObject roomsContainer, CellNeighborInfo neighborInfo)
    {
        GameObject roomObject;

        // TWO GATE: UP DOWN
        if (neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 90, 0);
        }
        // TWO GATE: LEFT RIGHT
        else if (!neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
        }
        // ONE GATE: DOWN
        else if (neighborInfo.Down && !neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, -90, 0);
        }
        // ONE GATE: UP
        else if (!neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 90, 0);
        }
        // ONE GATE: LEFT
        else if (!neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
        }
        // ONE GATE: RIGHT
        else if (!neighborInfo.Down && !neighborInfo.Up && !neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, -180, 0);
        }
        // TWO GATE L: UP RIGHT
        else if (!neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateLRoomPrefab, roomsContainer.transform);
        }
        // TWO GATE L: RIGHT DOWN
        else if (neighborInfo.Down && !neighborInfo.Up && !neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateLRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 90, 0);
        }
        // TWO GATE L: DOWN LEFT
        else if (neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateLRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 180, 0);
        }
        // TWO GATE L: UP LEFT
        else if (!neighborInfo.Down && neighborInfo.Up && neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(TwoGateLRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, -90, 0);
        }
        // THREE GATE: UP LEFT RIGHT
        else if (!neighborInfo.Down && neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(ThreeGateRoomPrefab, roomsContainer.transform);
        }
        // THREE GATE: UP RIGHT BOTTOM
        else if (neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(ThreeGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 90, 0);
        }
        // THREE GATE: LEFT RIGHT BOTTOM
        else if (neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(ThreeGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, 180, 0);
        }
        // THREE GATE: UP LEFT BOTTOM
        else if (neighborInfo.Down && neighborInfo.Up && neighborInfo.Left && !neighborInfo.Right)
        {
            roomObject = Instantiate(ThreeGateRoomPrefab, roomsContainer.transform);
            roomObject.transform.Rotate(0, -90, 0);
        }
        // FOUR GATE: UP RIGHT DOWN LEFT
        else if (neighborInfo.Down && neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
        {
            roomObject = Instantiate(FourGateRoomPrefab, roomsContainer.transform);
        }
        else
        {
            roomObject = Instantiate(FourGateRoomPrefab, roomsContainer.transform);
        }

        return roomObject;
    }
}