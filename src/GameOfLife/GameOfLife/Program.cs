using System;
using System.Threading;
using GameOfLife.Boards;
using GameOfLife.Rules;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Game of Life in C#");

            var board = ConwaysGameOfLife.GetBlankBoard(50, 50);
            //ConwaysGameOfLife.RandomiseBoard(board);

            var rules = ConwaysGameOfLife.GetRulesEngine();

            ConwaysGameOfLife.RenderPredefinedPattern(board, ConwaysGameOfLife.PredefinedPatterns.Exploder, (25, 25));

            Console.ReadLine();

            // Life loop
            while (true)
            {
                RenderBoard(board);

                board = rules.ApplyRulesToBoard(board);

                //Thread.Sleep(1_000/2); // tick
                Console.ReadLine();
            }
        }

        private static void RenderBoard(IBoard<ConwayCellState> board)
        {
            const string block = "\u25A0";

            Console.Clear();
            foreach (var cell in board.GetCells())
            {
                Console.SetCursorPosition(cell.X, cell.Y);
                if (cell.CellValue.IsAlive()) Console.Write(block);
            }
        }
    }
}