using System.Collections.Generic;

namespace GameOfLife.Boards
{
    public class Board<T> : IBoard<T>
    {
        private readonly T[] _data;
        public int Width { get; }
        public int Height { get; }

        public IEnumerable<BoardCell<T>> GetCells()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                // i = W * y + x
                var y = i / Height;
                var x = i - (y * Width);

                yield return new BoardCell<T>(i, x, y, this);
            }
        }

        public Board(int width, int height) : this(width, height, new T[width * height])
        {
        }

        protected Board(int width, int height, T[] data)
        {
            Width = width;
            Height = height;

            _data = data;
        }

        public void SetCells(params (int x, int y, T value)[] values)
        {
            foreach (var (x, y, value) in values)
            {
                SetCell(x, y, value);
            }
        }

        public T GetData(int i)
        {
            return _data[i];
        }

        public void SetData(int i, T value)
        {
            _data[i] = value;
        }

        public void SetCell(int x, int y, T value)
        {
            SetData(CoordToIndex(x, y), value);
        }

        public T GetCell(int x, int y)
        {
            x = WrapCoordinate(x, Width);
            y = WrapCoordinate(y, Height);

            return GetData(CoordToIndex(x, y));
        }

        private int CoordToIndex(int x, int y)
        {
            return y * Width + x;
        }

        private int WrapCoordinate(int value, int maxValue)
        {
            if (value < 0)
            {
                return maxValue + (value % maxValue);
            }

            return value % maxValue;
        }

        public IBoard<T> Snapshot()
        {
            var newData = new T[_data.Length];
            _data.CopyTo(newData, 0);
            return new Board<T>(Width, Height, newData);
        }
    }
}
