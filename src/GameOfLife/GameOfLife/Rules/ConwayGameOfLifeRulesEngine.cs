using GameOfLife.Boards;

namespace GameOfLife.Rules
{
    public class ConwayGameOfLifeRulesEngine : IBoardRulesEngine<ConwayCellState>
    {
        public IBoard<ConwayCellState> ExecuteRules(IBoard<ConwayCellState> board)
        {
            var newBoard = board.Snapshot();
            foreach (var cell in board.GetCells())
            {
                var count = CountNeighboursOfCell(board, cell);

                var currentState = cell.CellValue;

                // Any live cell with two/ three live neighbours lives.
                // Any dead cell with three live neighbours becomes a live cell.
                // All other live cells die in the next generation (the dead stay dead)
                var newState = (currentState, count) switch
                {
                    (ConwayCellState.Live, 2) => ConwayCellState.Live,
                    (_, 3) => ConwayCellState.Live,
                    _ => ConwayCellState.Dead
                };

                newBoard.SetCell(cell.X, cell.Y, newState);
            }

            return newBoard;
        }

        public int CountNeighboursOfCell(IBoard<ConwayCellState> board, IBoardCell<ConwayCellState> currentCell)
        {
            var count = 0;

            // Iterate the 8 cells around the current cell
            //  ╔═══════════╦════════════╦══════════╗
            //  ║ 1 (-1,-1) ║  2 (0,-1)  ║ 3 (1,-1) ║
            //  ╠═══════════╬════════════╬══════════╣
            //  ║ 4 (-1,0)  ║ Cell (0,0) ║ 5 (1,0)  ║
            //  ╠═══════════╬════════════╬══════════╣
            //  ║ 6 (-1,1)  ║ 7 (0,1)    ║ 8 (1,1)  ║
            //  ╚═══════════╩════════════╩══════════╝

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue; // Ignore our self...

                    var newX = currentCell.X + x;
                    var newY = currentCell.Y + y;
                    if (board.GetCell(newX, newY).IsAlive())
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }
    }
}
