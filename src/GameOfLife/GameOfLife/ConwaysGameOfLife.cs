using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameOfLife.Boards;
using GameOfLife.Rules;
using Microsoft.Extensions.FileProviders;

namespace GameOfLife
{
    public static class ConwaysGameOfLife
    {
        // ReSharper disable once InconsistentNaming
        public enum PredefinedPatterns
        {
            SmallExploder,
            XKCDRip,
            TenCellRow,
            Exploder,
            Glider,
            GosperGliderGun,
            LightweightSpaceship,
            Tumbler
        }

        private static readonly Dictionary<PredefinedPatterns, Lazy<bool[,]>> PredefinedPatternsCache;

        static ConwaysGameOfLife()
        {
            PredefinedPatternsCache = Enum.GetValues(typeof(PredefinedPatterns)).Cast<PredefinedPatterns>().ToDictionary(k => k, item => new Lazy<bool[,]>(() => ParseBitmapToPointsArray(item)));
        }

        private static bool[,] ParseBitmapToPointsArray(PredefinedPatterns item)
        {
            var path = $"Resources/{Enum.GetName(typeof(PredefinedPatterns), item)}.bmp";

            var embeddedProvider = new EmbeddedFileProvider(typeof(ConwaysGameOfLife).Assembly);
            using var reader = embeddedProvider.GetFileInfo(path).CreateReadStream();
            using var bmp = new Bitmap(reader);

            // Enumerable points is never accessed outside of method scope so safe
            // ReSharper disable once AccessToDisposedClosure
            var points =
                from bmpX in Enumerable.Range(0, bmp.Width)
                from bmpY in Enumerable.Range(0, bmp.Height)
                select new
                {
                    x = bmpX,
                    y = bmpY,
                    pixel = bmp.GetPixel(bmpX, bmpY) 
                };

            var bmpPoints = new bool[bmp.Width, bmp.Height];

            foreach (var point in points)
            {
                var isSet = !(point.pixel.R == 255 && point.pixel.G == 255 && point.pixel.B == 255); // Anything not white is set
                bmpPoints[point.x, point.y] = isSet;
            }

            return bmpPoints;
        }

        public static void RenderPredefinedPattern(IBoard<ConwayCellState> board, PredefinedPatterns item, (int x, int y) originPoint)
        {
            var points = PredefinedPatternsCache[item].Value;

            // Always a rank 2
            for (var x = 0; x < points.GetLength(0); x++)
            {
                for (var y = 0; y < points.GetLength(1); y++)
                {
                    board.SetCell(originPoint.x + x, originPoint.y + y, points[x, y] ? ConwayCellState.Live : ConwayCellState.Dead);
                }
            }
        }

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
    }
}
