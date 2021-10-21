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
        public RoomType RoomType;
        public List<GameObject> GameObjects;
    }

    public int MapWidth;
    public int MapHeight;
    public int Rooms;
    public GameObject WallPrefab;
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
        Queue<Cell> cells = this.maze.Generate();
        coroutine = StartCoroutine(SlowShow(cells));
    }

    /// <summary>
    ///  Delay display
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowShow(Queue<Cell> queue)
    {
        GameObject roomsContainer = new GameObject("Rooms");
        while (queue.Count > 0)
        {
            yield return new WaitForSecondsRealtime(ShowSpeed);

            Cell cell = queue.Dequeue();
            var roomObject = Instantiate(WallPrefab, roomsContainer.transform);
            var room = roomObject.GetComponent<Room>();

            rooms[cell.X, cell.Y] = roomObject;
            rooms[cell.X, cell.Y].transform.position = new Vector3(cell.X * room.Size, 0, cell.Y * room.Size);

            // start room
            if (cell.IsFirstGeneratedCell)
            {
                // set spawn object
                roomObject.name = "Spawn";
                room.Type = RoomType.Spawn;

                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }

            if (cell.Direction == Maze.Direction.Up)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            }
            else if (cell.Direction == Maze.Direction.Left)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 255, 0);
            }
            else if (cell.Direction == Maze.Direction.Right)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 255);
            }
            else if (cell.Direction == Maze.Direction.Down)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 0, 255);
            }

            // last room
            if (queue.Count == 0)
            {
                roomObject.name = "End";
                room.Type = RoomType.End;

                var lastCell = this.maze.GetCell(cell.X, cell.Y);
                lastCell.IsLastGeneratedCell = true;
                this.maze.SetCell(cell.X, cell.Y, lastCell);
            }
        }

        roomsContainer.transform.position = new Vector3(roomsContainer.transform.position.x - 42f * MapWidth / 2, 0, roomsContainer.transform.position.y - 42f * MapHeight / 2);

        print("End");
        StopCoroutine(coroutine);
    }
}