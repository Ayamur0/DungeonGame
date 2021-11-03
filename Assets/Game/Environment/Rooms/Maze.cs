using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.Game.Environment.Rooms
{
    [Serializable]
    public struct CellNeighborInfo
    {
        public bool Up;
        public bool Right;
        public bool Down;
        public bool Left;
    }

    public class Maze
    {
        /*
         * Using Randomized Prim's algorithm
         * Ref: https://en.wikipedia.org/wiki/Maze_generation_algorithm
         * Source inspired by: https://www.programmerall.com/article/2152455253/
         */

        public enum Direction
        {
            Uninitialized = -1,
            Down = 0,
            Up = 1,
            Left = 2,
            Right = 3,
        }

        [Serializable]
        public struct Cell
        {
            public int X;
            public int Y;
            public Direction Direction;
            public bool IsVisited;
            public bool IsUsed;
            public CellNeighborInfo NeighborInfo;
        }

        private readonly int width;
        private readonly int height;
        private readonly int rooms;
        private readonly Cell[,] cells;

        private readonly List<Cell> cellList = new List<Cell>();
        private readonly List<Cell> usedCells = new List<Cell>();

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
                    cells[i, j].IsUsed = false;
                    cells[i, j].NeighborInfo = new CellNeighborInfo();
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            return this.cells[x, y];
        }

        public Cell[] Generate()
        {
            // always spawn in center of the map
            int x = this.width / 2;
            int y = this.height / 2;

            cells[x, y].IsVisited = true;
            cells[x, y].IsUsed = true;

            AddCell(x, y);

            usedCells.Add(cells[x, y]);

            while (cellList.Count > 0)
            {
                int listIndex = Random.Range(0, cellList.Count);

                int newX = -1, newY = -1;
                switch (cellList[listIndex].Direction)
                {
                    case Direction.Down:
                        if (cellList[listIndex].Y + 1 < this.height && !cells[cellList[listIndex].X, cellList[listIndex].Y + 1].IsVisited)
                        {
                            newX = cellList[listIndex].X;
                            newY = cellList[listIndex].Y + 1;
                        }
                        break;

                    case Direction.Up:
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

                if (newX != -1 && newY != -1 && usedCells.Count < this.rooms)
                {
                    AddCell(newX, newY);

                    cells[cellList[listIndex].X, cellList[listIndex].Y].IsUsed = true;
                    cells[newX, newY].IsUsed = true;

                    usedCells.Add(cells[cellList[listIndex].X, cellList[listIndex].Y]);
                    usedCells.Add(cells[newX, newY]);

                    cells[cellList[listIndex].X, cellList[listIndex].Y].IsVisited = true;
                    cells[newX, newY].IsVisited = true;
                }

                cellList.RemoveAt(listIndex);
            }

            SetCellNeighborInfo();

            return this.usedCells.ToArray();
        }

        public void SetCellNeighborInfo()
        {
            for (int i = 0; i < usedCells.Count; i++)
            {
                var y = usedCells[i].Y;
                var x = usedCells[i].X;
                var cell = usedCells[i];

                if (y + 1 < this.height && this.cells[x, y + 1].IsUsed)
                {
                    cells[x, y].NeighborInfo.Up = true;
                    cell.NeighborInfo.Up = true;
                    usedCells[i] = cell;
                }

                if (y - 1 >= 0 && this.cells[x, y - 1].IsUsed)
                {
                    cells[x, y].NeighborInfo.Down = true;
                    cell.NeighborInfo.Down = true;
                    usedCells[i] = cell;
                }

                if (x - 1 >= 0 && this.cells[x - 1, y].IsUsed)
                {
                    cells[x, y].NeighborInfo.Left = true;
                    cell.NeighborInfo.Left = true;
                    usedCells[i] = cell;
                }

                if (x + 1 < this.width && this.cells[x + 1, y].IsUsed)
                {
                    cells[x, y].NeighborInfo.Right = true;
                    cell.NeighborInfo.Right = true;
                    usedCells[i] = cell;
                }
            }
        }

        private void AddCell(int x, int y)
        {
            if (y + 1 < this.height && !cells[x, y + 1].IsVisited)
            {
                cells[x, y + 1].Direction = Direction.Down;
                cellList.Add(cells[x, y + 1]);
            }

            if (y - 1 >= 0 && !cells[x, y - 1].IsVisited)
            {
                cells[x, y - 1].Direction = Direction.Up;
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