namespace GameOfLife.Boards
{
    public class BoardCell<T> : IBoardCell<T>
    {
        private readonly int _index;
        private readonly Board<T> _board;

        public BoardCell(int index, int x, int y, Board<T> board)
        {
            _index = index;
            _board = board;
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public T CellValue
        {
            get => _board.GetData(_index);
            set => _board.SetData(_index, value);
        }

        public override string ToString()
        {
            return $"({X},{Y}) {CellValue}";
        }
    }
}
