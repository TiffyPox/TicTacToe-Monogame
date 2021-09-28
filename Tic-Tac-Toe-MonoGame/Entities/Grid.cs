using System;
using System.Collections.Generic;

namespace TicTacToe.Entities
{
    public class Grid
    {
        private Cell[,] _cells;
        private readonly int _gridWidth;
        private readonly int _gridHeight;
        private readonly int _cellWidth;
        private readonly int _cellHeight;
        private Cell _selectedCell;

        private readonly List<(int, int)[]> _winConditions = new List<(int, int)[]>();

        public Grid(int gridWidth, int gridHeight, int cellWidth, int cellHeight)
        {
            _gridWidth = gridWidth;
            _gridHeight = gridHeight;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;

            CreateGrid();
            CreateWinConditions();
        }

        private void CreateWinConditions()
        {
            _winConditions.Add(new[] {(0, 0), (0, 1), (0, 2)}); // left column
            _winConditions.Add(new[] {(1, 0), (1, 1), (1, 2)}); // middle column
            _winConditions.Add(new[] {(2, 0), (2, 1), (2, 2)}); // right column
            _winConditions.Add(new[] {(0, 0), (1, 0), (2, 0)}); // top row
            _winConditions.Add(new[] {(0, 1), (1, 1), (2, 1)}); // middle row
            _winConditions.Add(new[] {(0, 2), (1, 2), (2, 2)}); // bottom row
            _winConditions.Add(new[] {(0, 0), (1, 1), (2, 2)}); // top left to bottom right
            _winConditions.Add(new[] {(0, 2), (1, 1), (2, 0)}); // top right to bottom left
        }

        internal int GetCellHeight() => _cellHeight;

        internal int GetCellWidth() => _cellWidth;

        private void CreateGrid()
        {
            _cells = new Cell[_gridWidth, _gridHeight];

            for (var i = 0; i < _gridWidth; ++i)
            {
                for (var j = 0; j < _gridHeight; ++j)
                {
                    _cells[i, j] = CreateCell(i, j);
                }
            }
        }

        private static Cell CreateCell(int x, int y) => new Cell(x, y);

        public void ForEachCell(Action<Cell> cb)
        {
            for (var i = 0; i < _gridWidth; ++i)
            {
                for (var j = 0; j < _gridHeight; ++j)
                {
                    cb(_cells[i, j]);
                }
            }
        }

        public Cell GetCellAt(int x, int y)
        {
            var cellX = (int) Math.Floor((float) x / _cellWidth);
            var cellY = (int) Math.Floor((float) y / _cellHeight);

            if (cellX < 0 || cellY < 0 || cellX >= _gridWidth || cellY >= _gridHeight)
            {
                return null;
            }

            return _cells[cellX, cellY];
        }

        internal int GetWidth() => _gridWidth;

        internal int GetHeight() => _gridHeight;

        public Cell GetSelectedCell() => _selectedCell;
        public void SetSelected(Cell cell) => _selectedCell = cell;

        public ((int, int)[], PlayerToken) CheckForWin()
        {
            foreach(var winCondition in _winConditions)
            {
                var isWinner = AreEqual(winCondition[0], winCondition[1], winCondition[2]);
                if (isWinner != null)
                {
                    return (winCondition, isWinner);
                }
            }
            return (null, null);
        }

        private PlayerToken AreEqual((int, int) p0, (int, int) p1, (int, int) p2)
        {
            var one = _cells[p0.Item1, p0.Item2];
            var two = _cells[p1.Item1, p1.Item2];
            var three =_cells[p2.Item1, p2.Item2];

            if (!one.IsTaken || !two.IsTaken || !three.IsTaken)
            {
                return null;
            }

            return one.GetToken().Id == two.GetToken().Id && one.GetToken().Id == three.GetToken().Id
                ? one.GetToken()
                : null;
        }

        public Cell GetCell(int x, int y) => _cells[x, y];

        public void Reset()
        {
            CreateGrid();
            CreateWinConditions();
        }
    }
}