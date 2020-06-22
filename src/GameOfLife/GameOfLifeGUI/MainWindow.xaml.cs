using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOfLife;
using GameOfLife.Boards;
using GameOfLife.Rules;

namespace GameOfLifeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var thr = new Thread(GameLoop);
            thr.Start();
        }

        private void Tick()
        {
            Thread.Sleep(100);
        }

        private void GameLoop(object? obj)
        {
            var board = ConwaysGameOfLife.GetBlankBoard(100, 100);
            var rules = ConwaysGameOfLife.GetRulesEngine();

            ConwaysGameOfLife.RenderPredefinedPattern(board, ConwaysGameOfLife.PredefinedPatterns.XKCDRip, (25, 25));

            var board2 = board;
            Dispatcher.Invoke(() => RenderBoard(board2));

            Thread.Sleep(1000);

            // Life loop
            while (true)
            {
                Tick();

                var board1 = board;
                Dispatcher.Invoke(() => RenderBoard(board1));

                board = rules.ExecuteRules(board);
            }
        }

        private void RenderBoard(IBoard<ConwayCellState> board)
        {
            GameCanvas.Children.Clear();

            var width = GameCanvas.ActualWidth;
            var height = GameCanvas.ActualHeight;

            var heightRatio = height / board.Height;
            var widthRatio = width / board.Width;

            //foreach (var cell in board.GetCells().Where(c => c.CellValue.IsAlive()))
            foreach (var cell in board.GetCells())
            {
                Rectangle rect;
                if (cell.CellValue.IsAlive())
                {
                    rect = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = new SolidColorBrush(Colors.Black),
                        Width = widthRatio,
                        Height = heightRatio
                    };
                }
                else
                {
                    rect = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        Fill = new SolidColorBrush(Colors.White),
                        Width = widthRatio,
                        Height = heightRatio
                    };
                }

                var canvasX = cell.X * widthRatio;
                var canvasY = cell.Y * heightRatio;
                Canvas.SetLeft(rect, canvasX);
                Canvas.SetTop(rect, canvasY);
                GameCanvas.Children.Add(rect);
            }
        }
    }
}
