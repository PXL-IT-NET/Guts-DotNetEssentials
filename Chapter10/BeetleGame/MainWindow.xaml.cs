using System;

using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace BeetleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Beetle _beetle;
        private int _xStart;
        private int _yStart;
        private DispatcherTimer timer;
        private DateTime _startTime;



        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer(); ;
            timer.Tick += Timer_Tick;
            InitializeBeetle();
            timer.Interval = TimeSpan.FromMilliseconds(100 / speedSlider.Value * sizeSlider.Value / 10);
            speedSlider.ValueChanged += SpeedSlider_ValueChanged;
            sizeSlider.ValueChanged += SizeSlider_ValueChanged;
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speedLabel.Content = $"{speedSlider.Value:0.0}";
            _beetle.Speed = speedSlider.Value;
            timer.Interval = TimeSpan.FromMilliseconds(100 / speedSlider.Value * sizeSlider.Value / 10);
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sizeLabel.Content = Convert.ToInt32(sizeSlider.Value);
            _beetle.Size = Convert.ToInt32(sizeSlider.Value);
            timer.Interval = TimeSpan.FromMilliseconds(100 / speedSlider.Value * sizeSlider.Value / 10);
            // indien 0.10 per stap => tik per 1/10 seconde voor de kleinste kever
            // aan 1 cm/seconde voortbeweegt.

        }

        private void GenerateStartPosition()
        {
            int maxX = Convert.ToInt32(paperCanvas.Width) - 30;
            int maxY = Convert.ToInt32(paperCanvas.Height) - 30;
            int middenX = Convert.ToInt32(paperCanvas.Width / 2);
            int middenY = Convert.ToInt32(paperCanvas.Height / 2);
            Random rand = new Random();
            _xStart = rand.Next(30, maxX);
            _yStart = rand.Next(30, maxY);
            while (ComputeDistance(_xStart, middenX, _yStart, middenY) < 100)
            {
                _xStart = rand.Next(30, maxX);
                _yStart = rand.Next(30, maxY);
            }
        }

        private double ComputeDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)startButton.Content == "Start")
            {
                _beetle.IsVisible = true;
                timer.Start();
                startButton.Content = "Stop";
                _startTime = DateTime.Now;
                speedSlider.IsEnabled = false;
                sizeSlider.IsEnabled = false;
                messageLabel.Background = new SolidColorBrush(Colors.White);
                messageLabel.Content = ($"");
            }
            else
            {
                speedSlider.IsEnabled = true;
                sizeSlider.IsEnabled = true;
                startButton.Content = "Start";
                timer.Stop();
                double distance = _beetle.ComputeDistance(_startTime, DateTime.Now);
                messageLabel.Content = ($"The total distance in meter {distance:0.00}");
                messageLabel.Background = new SolidColorBrush(Colors.BlueViolet);


            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _beetle.ChangePosition();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            speedSlider.Value = speedSlider.Minimum;
            sizeSlider.Value = sizeSlider.Minimum;
            speedSlider.IsEnabled = true;
            sizeSlider.IsEnabled = true;
            startButton.Content = "Start";
            _beetle.IsVisible = false;
            timer.Stop();
        }

        private void InitializeBeetle()
        {
            GenerateStartPosition();
            _beetle = new Beetle(paperCanvas, _xStart, _yStart, Convert.ToInt32(sizeSlider.Value))
            {
                IsVisible = false,
                Speed = speedSlider.Value
            };
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            _beetle.Right = false;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            _beetle.Up = true;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            _beetle.Up = false;
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            _beetle.Right = true;
        }
    }
};

