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

    /// <summary>
    /// Randomized Prim
    /// </summary>
    private void ShowRooms()
    {
        var cells = this.maze.Generate().ToArray();
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
            // var roomObject = Instantiate(WallPrefab, roomsContainer.transform);
            // var room = roomObject.GetComponent<Room>();

            // rooms[cell.X, cell.Y] = roomObject;
            // rooms[cell.X, cell.Y].transform.position = new Vector3(cell.X * room.Size, 0, cell.Y * room.Size);

            // start room
            if (i == 0)
            {
                roomObject = Instantiate(WallPrefab, roomsContainer.transform);
                // var room = roomObject.GetComponent<Room>();
                roomObject.name = "Spawn";
                //room.Type = RoomType.Spawn;

                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }

            if (roomObject == null)
            {
                if (cells[i].Direction == Maze.Direction.Up)
                {
                    //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 0, 255);
                    roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
                    roomObject.transform.Rotate(0, 90, 0);
                }
                else if (cells[i].Direction == Maze.Direction.Left)
                {
                    roomObject = Instantiate(TwoGateRoomPrefab, roomsContainer.transform);
                }
                else
                {
                    // check neighours

                    roomObject = Instantiate(WallPrefab, roomsContainer.transform);
                }
                /* else if (cell.Direction == Maze.Direction.Left)
                {

                }
                else if (cell.Direction == Maze.Direction.Right)
                {
                    //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 255);
                }
                else if (cell.Direction == Maze.Direction.Down)
                {
                    //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 0, 255);
                }*/

                // last room
                if (i == cells.Length - 1)
                {
                    roomObject.name = "End";

                    var lastCell = this.maze.GetCell(cells[i].X, cells[i].Y);
                    lastCell.IsLastGeneratedCell = true;
                    this.maze.SetCell(cells[i].X, cells[i].Y, lastCell);
                }
            }

            var room = roomObject.GetComponent<Room>();
            room.Direction = cells[i].Direction;
            rooms[cells[i].X, cells[i].Y] = roomObject;
            rooms[cells[i].X, cells[i].Y].transform.position = new Vector3(cells[i].X * room.Size, 0, cells[i].Y * room.Size);
        }

        roomsContainer.transform.position = new Vector3(roomsContainer.transform.position.x - 42f * MapWidth / 2, 0, roomsContainer.transform.position.y - 42f * MapHeight / 2);

        print("End");
        StopCoroutine(coroutine);
    }
}