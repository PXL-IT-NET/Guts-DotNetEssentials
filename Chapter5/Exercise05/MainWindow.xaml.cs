using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Exercise05
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void drawButton_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);

            DrawLogo(paperCanvas, brush, 10, 20);
            DrawLogo(paperCanvas, brush, 100, 100);
            DrawTriangle2(paperCanvas, brush, 100, 10, 40, 40);
            DrawTriangle2(paperCanvas, brush, 10, 100, 20, 60);
        }

        private void DrawTriangle2(Canvas drawingArea,
                                 SolidColorBrush brushToUse,
                                 double topX,
                                 double topY,
                                 double width,
                                 double height)
        {
            double rightCornerX, rightCornerY;
            rightCornerX = topX + width;
            rightCornerY = topY + height;

            DrawPolygon(drawingArea, brushToUse,
                         topX, topY,
                         topX, rightCornerY,
                         rightCornerX, rightCornerY);
        }

        private void DrawPolygon(Canvas drawingArea,
                                  SolidColorBrush brushToUse,
                                  double X1, double Y1,
                                  double X2, double Y2,
                                  double X3, double Y3)
        {
            Polygon poly = new Polygon
            {
                Stroke = brushToUse,
                StrokeThickness = 1
            };
            poly.Points.Add(new Point(X1, Y1));
            poly.Points.Add(new Point(X2, Y2));
            poly.Points.Add(new Point(X3, Y3));
            drawingArea.Children.Add(poly);
        }



        private void DrawLogo(Canvas drawingArea,
                              SolidColorBrush brushToUse,
                              double xPos,
                              double yPos)
        {
            DrawRectangle(drawingArea, brushToUse, xPos, yPos, 20);
            DrawRectangle(drawingArea, brushToUse, xPos, yPos, 40);
            DrawRectangle(drawingArea, brushToUse, xPos, yPos, 60);
        }

        private void DrawRectangle(Canvas drawingArea,
                                   SolidColorBrush brushToUse,
                                   double xPos,
                                   double yPos,
                                   double size)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = size,
                Height = size,
                Margin = new Thickness(xPos, yPos, 0, 0),
                Stroke = brushToUse
            };
            drawingArea.Children.Add(rectangle);
        }
    }
}
