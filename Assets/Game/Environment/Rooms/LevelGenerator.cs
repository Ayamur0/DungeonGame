using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Environment.Rooms;
using UnityEngine;
using Cell = Assets.Game.Environment.Rooms.Maze.Cell;
using Random = UnityEngine.Random;

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
    public List<RoomDictionary> RoomList = new List<RoomDictionary>();

    [Serializable]
    public struct RoomDictionary
    {
        public RoomType Type;
        public List<GameObject> Prefabs;
    }

    private Maze maze;
    private GameObject[,] rooms;

    private void Start()
    {
        this.maze = new Maze(this.MapWidth, this.MapHeight, this.Rooms);
        this.rooms = new GameObject[MapWidth, MapHeight];

        ShowRooms();
    }

    private void ShowRooms()
    {
        var cells = this.maze.Generate();
        CreateCells(cells);
    }

    private void CreateCells(Cell[] cells)
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
                roomObject.tag = "Spawn";
                room.Type = RoomType.Spawn;

                // open room doors
                room.OpenGates();
            }
            else if (i == cells.Length - 1)
            {
                roomObject.name = "Exit";
                roomObject.tag = "Exit";
                room.Type = RoomType.Exit;
            }
            else
            {
                // set random object type (not spawn and exit)
                //var roomTypes = Enum.GetValues(typeof(RoomType));
                //var rndRoomType = (RoomType)roomTypes.GetValue(Random.Range(2, roomTypes.Length));
                room.Type = PickRandomRoomType();
            }
        }

        foreach (var roomObject in rooms)
        {
            if (roomObject)
            {
                var room = roomObject.GetComponent<Room>();
                room.NeighborInfos = maze.GetCell((int)room.CellPosition.x, (int)room.CellPosition.y).NeighborInfo;
                CreateRoomContent(roomObject, room.Type);
            }
        }

        // center room container
        var containerPos = roomsContainer.transform.position;
        var pos = new Vector3(containerPos.x - roomSize * MapWidth / 2, 0, containerPos.y - roomSize * MapHeight / 2);
        roomsContainer.transform.position = pos;
    }

    private RoomType PickRandomRoomType()
    {
        var probability = Random.Range(0, 100);
        if (probability < 5)
        {
            return RoomType.Shop;
        }
        else if (probability >= 5 && probability <= 30)
        {
            return RoomType.Explore;
        }

        return RoomType.Battle;
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

    public void CreateRoomContent(GameObject roomObj, RoomType type)
    {
        if (this.RoomList.Count > 0)
        {
            foreach (var room in RoomList)
            {
                if (room.Type == type && room.Prefabs != null)
                {
                    int listIndex = Random.Range(0, room.Prefabs.Count);
                    var prefab = room.Prefabs[listIndex];
                    if (prefab != null)
                    {
                        var roomContentObj = Instantiate(prefab, roomObj.transform);
                    }
                }
            }
        }
    }
}