using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Assets.Game.Environment.Rooms
{
    public class Maze
    {
        // https://www.programmerall.com/article/2152455253/
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
            public bool IsFirstGeneratedCell;
            public bool IsLastGeneratedCell;
        }

        private readonly int width;
        private readonly int height;
        private readonly int rooms;
        private readonly Cell[,] cells;

        private readonly List<Cell> cellList = new List<Cell>();
        private readonly Queue<Cell> cellQueue = new Queue<Cell>();

        public Maze(int width, int height, int rooms)
        {
            this.width = width;
            this.height = height;
            this.rooms = rooms;

            this.cells = new Cell[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cells[i, j].X = i;
                    cells[i, j].Y = j;
                    cells[i, j].Direction = Direction.Uninitialized;
                    cells[i, j].IsVisited = false;
                    cells[i, j].IsFirstGeneratedCell = false;
                    cells[i, j].IsLastGeneratedCell = false;
                }
            }

            Debug.Log(cells.Length);
        }

        public Cell GetCell(int x, int y)
        {
            return this.cells[x, y];
        }

        public void SetCell(int x, int y, Cell cell)
        {
            this.cells[x, y] = cell;
        }

        public Cell[] GetCells()
        {
            return this.cellQueue.ToArray();
        }

        public Queue<Cell> Generate()
        {
            // always spawn in center of the map
            int x = this.width / 2;
            int y = this.height / 2;

            cells[x, y].IsVisited = true;
            cells[x, y].IsFirstGeneratedCell = true;

            AddNewCellsToList(x, y);

            cellQueue.Enqueue(cells[x, y]);

            while (cellList.Count > 0)
            {
                int listIndex = Random.Range(0, cellList.Count);

                int newX = -1, newY = -1;
                switch (cellList[listIndex].Direction)
                {
                    case Direction.Up:
                        if (cellList[listIndex].Y + 1 < this.height && !cells[cellList[listIndex].X, cellList[listIndex].Y + 1].IsVisited)
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
                        if (cellList[listIndex].X + 1 < this.width && !cells[cellList[listIndex].X + 1, cellList[listIndex].Y].IsVisited)
                        {
                            newX = cellList[listIndex].X + 1;
                            newY = cellList[listIndex].Y;
                        }
                        break;

                    default:
                        throw new Exception(cellList[listIndex].X + "_" + cellList[listIndex].Y + " Fail");
                }

                if (newX != -1 && newY != -1 && cellQueue.Count < this.rooms)
                {
                    AddNewCellsToList(newX, newY);

                    cellQueue.Enqueue(cells[cellList[listIndex].X, cellList[listIndex].Y]);
                    cellQueue.Enqueue(cells[newX, newY]);

                    cells[cellList[listIndex].X, cellList[listIndex].Y].IsVisited = true;
                    cells[newX, newY].IsVisited = true;
                }

                cellList.RemoveAt(listIndex);
            }

            return this.cellQueue;
        }

        /// <summary>
        ///  Add new cell to the list
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddNewCellsToList(int x, int y)
        {
            if (y + 1 < this.height && !cells[x, y + 1].IsVisited)
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

            if (x + 1 < this.width && !cells[x + 1, y].IsVisited)
            {
                cells[x + 1, y].Direction = Direction.Right;
                cellList.Add(cells[x + 1, y]);
            }
        }
    }
}