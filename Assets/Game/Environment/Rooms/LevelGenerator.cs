using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Game.Environment.Rooms;
using UnityEngine;
using Cell = Assets.Game.Environment.Rooms.Maze.Cell;

public class LevelGenerator : MonoBehaviour
{
    [Serializable]
    public struct RoomContainer
    {
        public Maze.Direction Direction;
        public List<GameObject> GameObjects;
    }

    public int MapWidth;
    public int MapHeight;
    public int Rooms;
    public GameObject WallPrefab;
    public GameObject TwoGateRoomPrefab;
    public GameObject OneGateRoomPrefab;
    public float ShowSpeed = 0.25f;

    public List<RoomContainer> RoomContainers = new List<RoomContainer>();

    private Maze maze;
    private GameObject[,] rooms;

    private Coroutine coroutine;

    private void Start()
    {
        this.maze = new Maze(this.MapWidth, this.MapHeight, this.Rooms);
        this.rooms = new GameObject[MapWidth, MapHeight];

        ShowRooms();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    /// <summary>
    /// Randomized Prim
    /// </summary>
    private void ShowRooms()
    {
        var cells = this.maze.Generate();
        coroutine = StartCoroutine(SlowShow(cells));
    }

    /// <summary>
    ///  Delay display
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowShow(Cell[] cells)
    {
        GameObject roomsContainer = new GameObject("Rooms");

        for (int i = 0; i < cells.Length; i++)
        {
            yield return new WaitForSecondsRealtime(ShowSpeed);

            GameObject roomObject = null;

            var neighborInfo = cells[i].NeighborInfo;

            if (neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
            {
                roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
                roomObject.transform.Rotate(0, 90, 0);
            }
            else if (!neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
            {
                roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
            }
            else if (neighborInfo.Down && neighborInfo.Up && neighborInfo.Left && neighborInfo.Right)
            {
                roomObject = Instantiate(WallPrefab, roomsContainer.transform);
            }
            else if (neighborInfo.Down && !neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
            {
                roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
                roomObject.transform.Rotate(0, -90, 0);
            }
            else if (!neighborInfo.Down && neighborInfo.Up && !neighborInfo.Left && !neighborInfo.Right)
            {
                roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
            }
            else if (!neighborInfo.Down && !neighborInfo.Up && neighborInfo.Left && !neighborInfo.Right)
            {
                roomObject = Instantiate(OneGateRoomPrefab, roomsContainer.transform);
            }
            else
            {
                // start room
                if (i == 0)
                {
                    roomObject = Instantiate(WallPrefab, roomsContainer.transform);
                    roomObject.name = "Spawn";
                    //room.Type = RoomType.Spawn;
                }

                if (roomObject == null)
                {
                    roomObject = Instantiate(WallPrefab, roomsContainer.transform);
                }

                // last room
                if (i == cells.Length - 1)
                {
                    roomObject.name = "End";
                }
            }

            var room = roomObject.GetComponent<Room>();
            room.CellPosition = new Vector2(cells[i].X, cells[i].Y);
            rooms[cells[i].X, cells[i].Y] = roomObject;
            rooms[cells[i].X, cells[i].Y].transform.position = new Vector3(cells[i].X * room.Size, 0, cells[i].Y * room.Size);
        }

        foreach (var roomObject in rooms)
        {
            if (roomObject)
            {
                var room = roomObject.GetComponent<Room>();
                room.NeighborInfos = maze.GetCell((int)room.CellPosition.x, (int)room.CellPosition.y).NeighborInfo;
            }
        }

        roomsContainer.transform.position = new Vector3(roomsContainer.transform.position.x - 42f * MapWidth / 2, 0, roomsContainer.transform.position.y - 42f * MapHeight / 2);

        print("End");
        StopCoroutine(coroutine);
    }
}