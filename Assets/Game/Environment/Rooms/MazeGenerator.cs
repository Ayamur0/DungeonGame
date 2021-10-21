using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.programmerall.com/article/2152455253/

public class MazeGenerator : MonoBehaviour
{
    public enum Direction
    {
        Uninitialized = -1,
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    public struct Cell
    {
        public int X;
        public int Y;
        public Direction Direction;
        public bool IsVisited;
        public bool IsLastRoom;
        public bool IsSpawn;
    }

    public int MapWidth;
    public int MapHeight;
    public int Rooms;
    public GameObject WallPrefab;
    public float ShowSpeed = 0.25f;

    private Cell[,] cells;
    private GameObject[,] rooms;

    private readonly List<Cell> cellList = new List<Cell>();
    private readonly Queue<Cell> cellQueue = new Queue<Cell>();

    private Coroutine coroutine;

    private void Start()
    {
        CreateAndInit();
        DoRandomizedPrim();
    }

    /// <summary>
    /// Creation and initialization
    /// </summary>
    private void CreateAndInit()
    {
        rooms = new GameObject[MapWidth, MapHeight];
        cells = new Cell[MapWidth, MapHeight];

        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                rooms[i, j] = null;

                cells[i, j].X = i;
                cells[i, j].Y = j;
                cells[i, j].Direction = Direction.Uninitialized;
                cells[i, j].IsVisited = false;
                cells[i, j].IsSpawn = false;
                cells[i, j].IsLastRoom = false;
            }
        }
    }

    /// <summary>
    /// Randomized Prim
    /// </summary>
    private void DoRandomizedPrim()
    {
        // always spawn in center of the map
        int x = MapWidth / 2;
        int y = MapHeight / 2;

        cells[x, y].IsVisited = true;
        cells[x, y].IsSpawn = true;

        AddNewCellsToList(x, y);

        cellQueue.Enqueue(cells[x, y]);

        while (cellList.Count > 0)
        {
            int listIndex = Random.Range(0, cellList.Count);

            int newX = -1, newY = -1;
            switch (cellList[listIndex].Direction)
            {
                case Direction.Up:
                    if (cellList[listIndex].Y + 1 < MapHeight && !cells[cellList[listIndex].X, cellList[listIndex].Y + 1].IsVisited)
                    {
                        newX = cellList[listIndex].X;
                        newY = cellList[listIndex].Y + 1;
                    }
                    break;

                case Direction.Down:
                    if (cellList[listIndex].Y - 1 >= 0 && !cells[cellList[listIndex].X, cellList[listIndex].Y - 1].IsVisited)
                    {
                        newX = cellList[listIndex].X;
                        newY = cellList[listIndex].Y - 1;
                    }
                    break;

                case Direction.Left:
                    if (cellList[listIndex].X - 1 >= 0 && !cells[cellList[listIndex].X - 1, cellList[listIndex].Y].IsVisited)
                    {
                        newX = cellList[listIndex].X - 1;
                        newY = cellList[listIndex].Y;
                    }
                    break;

                case Direction.Right:
                    if (cellList[listIndex].X + 1 < MapWidth && !cells[cellList[listIndex].X + 1, cellList[listIndex].Y].IsVisited)
                    {
                        newX = cellList[listIndex].X + 1;
                        newY = cellList[listIndex].Y;
                    }
                    break;

                default:
                    print(cellList[listIndex].X + "_" + cellList[listIndex].Y + " Fail");
                    break;
            }

            if (newX != -1 && newY != -1 && cellQueue.Count < this.Rooms)
            {
                AddNewCellsToList(newX, newY);

                cellQueue.Enqueue(cells[cellList[listIndex].X, cellList[listIndex].Y]);
                cellQueue.Enqueue(cells[newX, newY]);

                cells[cellList[listIndex].X, cellList[listIndex].Y].IsVisited = true;
                cells[newX, newY].IsVisited = true;
            }

            cellList.RemoveAt(listIndex);
        }

        int items = cellQueue.Count;
        print(items);
        coroutine = StartCoroutine(SlowShow());
    }

    /// <summary>
    ///  Add new cell to the list
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void AddNewCellsToList(int x, int y)
    {
        if (y + 1 < MapHeight && !cells[x, y + 1].IsVisited)
        {
            cells[x, y + 1].Direction = Direction.Up;
            cellList.Add(cells[x, y + 1]);
        }

        if (y - 1 >= 0 && !cells[x, y - 1].IsVisited)
        {
            cells[x, y - 1].Direction = Direction.Down;
            cellList.Add(cells[x, y - 1]);
        }

        if (x - 1 >= 0 && !cells[x - 1, y].IsVisited)
        {
            cells[x - 1, y].Direction = Direction.Left;
            cellList.Add(cells[x - 1, y]);
        }

        if (x + 1 < MapWidth && !cells[x + 1, y].IsVisited)
        {
            cells[x + 1, y].Direction = Direction.Right;
            cellList.Add(cells[x + 1, y]);
        }
    }

    /// <summary>
    ///  Delay display
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowShow()
    {
        GameObject roomsContainer = new GameObject("Rooms");
        while (cellQueue.Count > 0)
        {
            yield return new WaitForSecondsRealtime(ShowSpeed);

            Cell cell = cellQueue.Dequeue();
            rooms[cell.X, cell.Y] = Instantiate(WallPrefab, roomsContainer.transform);
            var room = rooms[cell.X, cell.Y].GetComponent<Room>();
            rooms[cell.X, cell.Y].transform.position = new Vector3(cell.X * room.Size, 0, cell.Y * room.Size);

            // start room
            if (cell.IsSpawn)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }

            if (cell.Direction == Direction.Up)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            }
            else if (cell.Direction == Direction.Left)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 255, 0);
            }
            else if (cell.Direction == Direction.Right)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(0, 255, 255);
            }
            else if (cell.Direction == Direction.Down)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 0, 255);
            }

            // last room
            if (cellQueue.Count == 0)
            {
                //rooms[cell.X, cell.Y].GetComponent<Renderer>().material.color = new Color(255, 0, 0);
                cells[cell.X, cell.Y].IsLastRoom = true;
            }
        }

        roomsContainer.transform.position = new Vector3(roomsContainer.transform.position.x - 42f * MapWidth / 2, 0, roomsContainer.transform.position.y - 42f * MapHeight / 2);

        print("End");
        StopCoroutine(coroutine);
    }
}