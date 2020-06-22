namespace GameOfLife.Boards
{
    public interface IBoardCell<T>
    {
        int X { get; }
        int Y { get; }
        T CellValue { get; set; }
    }
}