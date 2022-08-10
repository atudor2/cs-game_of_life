using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameOfLife;
using GameOfLife.Boards;
using GameOfLife.Rules;

namespace GameOfLifeGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int BaseTickSpeed = 50;
        private const int BoardSize = 100;

        private readonly WriteableBitmap _bitmap;
        private readonly Thread _loopThread;
        private readonly IBoardRulesEngine<ConwayCellState> _rules;
        private IBoard<ConwayCellState> _board = null!;
        private bool _running;
        private readonly object _boardLock = new();
        private int _tickSpeed = BaseTickSpeed;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int TickSpeed
        {
            get => _tickSpeed;
            set
            {
                _tickSpeed = value;
                OnPropertyChanged();
            }
        }

        public List<string> Patterns { get; set; }

        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            _bitmap = BitmapFactory.New(512, 512);
            GridImage.Source = _bitmap;

            _loopThread = new Thread(GameLoop);
            _rules = ConwaysGameOfLife.GetRulesEngine();

            ResetBoard();

            Patterns = new List<string>(Enum.GetNames(typeof(ConwaysGameOfLife.PredefinedPatterns)));
            ComboBox.ItemsSource = Patterns;
        }

        private void ResetBoard()
        {
            LockBoard(() => _board = ConwaysGameOfLife.GetBlankBoard(BoardSize, BoardSize));
        }

        private void RandomiseBoard()
        {
            LockBoard(() => ConwaysGameOfLife.RandomiseBoard(_board));
        }

        private void Tick()
        {
            Thread.Sleep(TickSpeed);
        }

        private void GameLoop(object? obj)
        {
            // Life loop
            while (true)
            {
                Dispatcher.Invoke(RenderBoard);
                Tick();

                if (_running)
                {
                    LockBoard(() => _board = _rules.ExecuteRules(_board));
                }
            }
        }

        private void RenderBoard()
        {
            var board = _board;

            var width = _bitmap.PixelHeight;
            var height = _bitmap.PixelHeight;

            var heightRatio = height / board.Height;
            var widthRatio = width / board.Width;

            using (_bitmap.GetBitmapContext())
            {
                _bitmap.Clear(Colors.White);

                foreach (var cell in board.GetCells())
                {
                    var rectX = cell.X * widthRatio;
                    var rectY = cell.Y * heightRatio;

                    var x1 = rectX;
                    var x2 = rectX + widthRatio;
                    var y1 = rectY;
                    var y2 = rectY + heightRatio;

                    if (cell.CellValue.IsAlive())
                    {
                        _bitmap.FillRectangle(x1, y1, x2, y2, Colors.Black);
                    }
                    else
                    {
                        _bitmap.DrawRectangle(x1, y1, x2, y2, Colors.Black);
                    }
                }
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _loopThread.Start();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _running = !_running;
        }

        private void OnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard();
        }

        private void OnRandomise_Click(object sender, RoutedEventArgs e)
        {
            RandomiseBoard();
        }

        private void OnInsertPattern_Click(object sender, RoutedEventArgs e)
        {
            var selectedValue = ComboBox.SelectedValue?.ToString();
            if (selectedValue is null) return;

            var pattern = Enum.Parse<ConwaysGameOfLife.PredefinedPatterns>(selectedValue, true);
            LockBoard(() => ConwaysGameOfLife.RenderPredefinedPattern(_board, pattern, (25, 25)));
        }

        private void LockBoard(Action action)
        {
            lock (_boardLock)
            {
                action();
            }
        }

        private void Slider_valueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TickSpeed = (int)(SpeedSlider.Value * BaseTickSpeed);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
