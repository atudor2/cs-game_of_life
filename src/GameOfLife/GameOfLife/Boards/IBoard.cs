using System.Collections.Generic;

namespace GameOfLife.Boards
{
    public interface IBoard<T>
    {
        int Width { get; }
        int Height { get; }
        IEnumerable<BoardCell<T>> GetCells();
        void SetCell(int x, int y, T value);
        void SetCells(params (int x, int y, T value)[] values);
        T GetCell(int x, int y);
        IBoard<T> Snapshot();
    }
}