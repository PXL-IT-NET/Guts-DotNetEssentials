using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BeetleGame
{
    public class Beetle
    {
        private Canvas _canvas;
        private int _size;
        private Ellipse _body;
        public double Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool IsVisible
        {
            set
            {
                if (value)
                {
                    _body.Visibility = Visibility.Visible;

                }
                else
                {
                    _body.Visibility = Visibility.Hidden;
                }
            }
        }

        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                ChangeSize();
            }
        }


        public Beetle(Canvas canvas, int x, int y, int size)
        {
            X = x; Right = true;
            Up = true;
            Y = y;
            _size = size;
            _canvas = canvas;
            CreateBeetle();
        }

        private void CreateBeetle()
        {
            _body = new Ellipse
            {
                Margin = new Thickness(X - Size / 2, Y - Size / 2, 0, 0),
                Width = Size,
                Height = Size,
                Fill = new SolidColorBrush(Colors.Red)
            };
            _canvas.Children.Add(_body);


        }

        private void ChangeSize()
        {

            _body.Margin = new Thickness(X - _size / 2, Y - _size / 2, 0, 0);
            _body.Width = _size;
            _body.Height = _size;

        }

        public void ChangePosition()
        {
            if (Speed != 0)
            {
                if (Right)
                {
                    X = X + 1;
                }
                else
                {
                    X = X - 1;
                }
                if (Up)
                {
                    Y = Y - 1;
                }
                else
                {
                    Y = Y + 1;
                }
                _body.Margin = new Thickness(X - Size / 2, Y - Size / 2, 0, 0);
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {   // linkerzijde of rechterzijde => links of rechts wisselen
            if (X - Size / 2 == 0 || X + Size / 2 == _canvas.Width)
            {
                Right = !Right;
            }
            // boven of onderkant => omhoog omlaag wisselen
            if (Y - Size / 2 == 0 || Y + Size / 2 == _canvas.Height)
            {
                Up = !Up;
            }
        }

        public double ComputeDistance(DateTime time1, DateTime time2)
        {
            TimeSpan time = time2 - time1;
            int seconds = time.Seconds;
            double distance = seconds * Speed / 100;
            return distance;
        }

    }
}
