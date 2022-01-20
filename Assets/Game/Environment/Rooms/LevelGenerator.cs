using Assets.Game.Environment.Rooms;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cell = Assets.Game.Environment.Rooms.Maze.Cell;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject FourGateRoomPrefab;
    public GameObject TwoGateRoomPrefab;
    public GameObject OneGateRoomPrefab;
    public GameObject TwoGateLRoomPrefab;
    public GameObject ThreeGateRoomPrefab;

    public List<RoomDictionary> RoomList = new List<RoomDictionary>();

    private int currentShops = 0;
    private int currentExplore = 0;

    private LevelManager lvlManager;

    public LevelSettings Settings = new LevelSettings(1, 2);

    [Serializable]
    public struct RoomDictionary
    {
        public RoomType Type;
        public List<GameObject> Prefabs;
    }

    [Serializable]
    public class LevelSettings
    {
        public int MaxShops = 1;
        public int MaxExplore = 2;
        public int MapWidth = 20;
        public int MapHeight = 20;
        public int Rooms = 20;

        public LevelSettings(int maxShops, int maxExplore, int rooms = 20, int mapWidth = 20, int mapHeight = 20)
        {
            this.MaxShops = maxShops;
            this.MaxExplore = maxExplore;
            this.Rooms = rooms;
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;
        }
    }

    private Maze maze;
    private GameObject[,] rooms;

    public Action RoomsGenerated;

    public GameObject[,] GetRooms()
    {
        return this.rooms;
    }

    private void Start()
    {
        this.lvlManager = this.GetComponent<LevelManager>();
        GenerateLevel(this.Settings);
        this.lvlManager.EnableNeighborRooms();
    }

    public void GenerateLevel(LevelSettings settings)
    {
        if (settings.MapHeight <= 0 || settings.MapWidth <= 0 || settings.Rooms < 4)
        {
            throw new Exception("Invalid Level Settings");
        }

        this.currentShops = 0;
        this.currentExplore = 0;

        this.maze = new Maze(settings.MapWidth, settings.MapHeight, settings.Rooms);
        this.rooms = new GameObject[settings.MapWidth, settings.MapHeight];
        var roomContainer = GameObject.FindGameObjectWithTag("RoomContainer");
        if (roomContainer)
        {
            Destroy(roomContainer);
        }

        var cells = this.maze.Generate();
        CreateCells(cells);

        this.Settings = settings;

        this.RoomsGenerated?.Invoke();

    }

    private void CreateCells(Cell[] cells)
    {
        GameObject roomsContainer = new GameObject("Rooms");
        roomsContainer.tag = "RoomContainer";
        Vector3 spawnPos = Vector3.zero;

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

            room.LevelManager = this.lvlManager;

            // start room
            if (i == 0)
            {
                roomObject.name = "Spawn";
                roomObject.tag = "Spawn";
                room.Type = RoomType.Spawn;

                this.lvlManager.SetActiveRoom(roomObject);

                spawnPos = roomObject.transform.position;

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
                room.Type = PickRandomRoomType(cells[i].X, cells[i].Y);
                room.OpenGates();
            }

            roomObject.name = room.Type.ToString();
        }

        foreach (var roomObject in rooms)
        {
            if (roomObject)
            {
                var room = roomObject.GetComponent<Room>();
                room.NeighborInfos = maze.GetCell((int)room.CellPosition.x, (int)room.CellPosition.y).NeighborInfo;
                CreateRoomContent(roomObject, room.Type);

                if (room.Type != RoomType.Spawn && this.lvlManager.HideRooms)
                {
                    roomObject.SetActive(false);
                }
            }
        }

        // center room container
        var containerPos = roomsContainer.transform.position;
        var pos = new Vector3(containerPos.x - roomSize * Settings.MapWidth / 2, 0, containerPos.y - roomSize * Settings.MapHeight / 2);
        roomsContainer.transform.position = pos;
    }

    private RoomType PickRandomRoomType(int cellX, int cellY)
    {
        // 3 = Room Types
        var probability = Random.Range(1, 3);

        if (probability == 1 && this.currentShops < Settings.MaxShops && !HasSameNeighbor(cellX, cellY, RoomType.Shop))
        {
            this.currentShops++;
            return RoomType.Shop;
        }
        else if (probability == 2 && this.currentExplore < Settings.MaxExplore && !HasSameNeighbor(cellX, cellY, RoomType.Explore))
        {
            this.currentExplore++;
            return RoomType.Explore;
        }

        return RoomType.Battle;
    }

    private bool HasSameNeighbor(int cellX, int cellY, RoomType roomType)
    {
        bool equal = false;

        // right
        if (cellX + 1 < this.rooms.Length)
        {
            equal = IsEqualRoomType(cellX + 1, cellY, roomType);

            if (equal)
                return true;
        }

        // left
        if (cellX - 1 >= 0)
        {
            equal = IsEqualRoomType(cellX - 1, cellY, roomType);

            if (equal)
                return true;
        }

        // down
        if (cellY + 1 < this.rooms.Length)
        {
            equal = IsEqualRoomType(cellX, cellY + 1, roomType);

            if (equal)
                return true;
        }

        // up
        if (cellY - 1 >= 0)
        {
            equal = IsEqualRoomType(cellX, cellY - 1, roomType);

            if (equal)
                return true;
        }

        return false;
    }

    private bool IsEqualRoomType(int cellX, int cellY, RoomType roomType)
    {
        var gameObj = this.rooms[cellX, cellY];
        if (gameObj)
        {
            var rightRoomType = gameObj.GetComponent<Room>().Type;
            return rightRoomType == roomType;
        }

        return false;
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
                        roomContentObj.name = "Content";
                    }
                }
            }
        }
    }
}