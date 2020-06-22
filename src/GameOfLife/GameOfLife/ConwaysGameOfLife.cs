using System;
using System.Drawing;
using System.Linq;
using GameOfLife.Boards;
using GameOfLife.Rules;

namespace GameOfLife
{
    public static class ConwaysGameOfLife
    {
        public static IBoardRulesEngine<ConwayCellState> GetRulesEngine()
        {
            return new ConwayGameOfLifeRulesEngine();
        }

        public static IBoard<ConwayCellState> GetBlankBoard(int width, int height)
        {
            return new Board<ConwayCellState>(width, height);
        }

        public static void RandomiseBoard(IBoard<ConwayCellState> board)
        {
            var random = new Random();

            foreach (var cell in board.GetCells())
            {
                cell.CellValue = random.NextDouble() > 0.5 ? ConwayCellState.Live : ConwayCellState.Dead; // Coin flip
            }
        }

        public static void RenderSmallExploder(IBoard<ConwayCellState> board, (int x, int y) centre)
        {
            var (x, y) = centre;

            // Small exploder:
            board.SetCells(
                (x, y, ConwayCellState.Dead), // Centre
                            (x, y + 1, ConwayCellState.Live),
                            (x, y - 1, ConwayCellState.Live),
                            (x - 1, y - 1, ConwayCellState.Live),
                            (x + 1, y - 1, ConwayCellState.Live),
                            (x, y - 2, ConwayCellState.Live),
                            (x - 1, y, ConwayCellState.Live),
                            (x + 1, y, ConwayCellState.Live)
            );
        }
        public static void RenderGlider(IBoard<ConwayCellState> board, (int x, int y) centre)
        {
            var (x, y) = centre;

            // Glider:
            board.SetCells(
                (x, y, ConwayCellState.Dead), // Centre
                (x, y - 1, ConwayCellState.Live),
                (x + 1, y, ConwayCellState.Live),

                (x - 1, y + 1, ConwayCellState.Live),
                (x, y + 1, ConwayCellState.Live),
                (x + 1, y + 1, ConwayCellState.Live)
            );
        }
    }
}
