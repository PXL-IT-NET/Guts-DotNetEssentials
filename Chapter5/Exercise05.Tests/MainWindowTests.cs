using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise05.Tests
{
    [ExerciseTestFixture("dotNet1", "H05", "Exercise05", @"Exercise05\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _testWindow;
        private Canvas _canvas;
        private Button _button;

        [SetUp]
        public void Setup()
        {
            _testWindow = new MainWindow();
            Grid grid = (Grid)_testWindow.Content;

            _canvas = grid.FindVisualChildren<Canvas>().ToList().FirstOrDefault();
            _button = grid.FindVisualChildren<Button>().ToList().FirstOrDefault();
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Close();
        }

        [MonitoredTest("Should have an empty canvas and a draw button"), Order(1)]
        public void _1_ShouldHaveAnEmptyCanvasAndADrawButton()
        {
            AssertButtonAndCanvasArePresent();
            Assert.That(_canvas.Children.Count, Is.EqualTo(0), () => "At the start, the canvas should be empty.");
        }

        [MonitoredTest("Should make use of the Polygon class"), Order(2)]
        public void _2_ShouldMakeUseOfThePolygonclass()
        {
            var sourceCode = Solution.Current.GetFileContent(@"Exercise05\MainWindow.xaml.cs");
            sourceCode = CodeCleaner.StripComments(sourceCode);

            Assert.That(sourceCode, Contains.Substring("new Polygon"), 
                () => "No code found where an instance of the Polygon class is created.");
            Assert.That(sourceCode, Contains.Substring("new Point(").Or.Contains("new System.Windows.Point("), 
                () => "No code found where instances op the Point class are created.");
            Assert.That(sourceCode, Contains.Substring(".Add("), 
                () => "No code found where points are added to the Polygon instance.");
        }

        [MonitoredTest("Should draw triangles and rectangles on the canvas after the draw button is clicked"), Order(3)]
        public void _3_ShouldDrawTrianglesAndRectanglesOnTheCanvasAfterTheButtonIsClicked()
        {
            AssertButtonAndCanvasArePresent();

            _button.FireClickEvent();

            var allChildren = _canvas.Children.Cast<object>().ToList();
            var polygons = allChildren.OfType<Polygon>().ToList();
            var rectangles = allChildren.OfType<Rectangle>().ToList();

            Assert.That(rectangles.Count, Is.GreaterThan(0), () => "Could not find any rectangles.");
            Assert.That(polygons.Count, Is.GreaterThan(0), () => "Could not find any polygons.");
            Assert.That(polygons, Has.All.With.Matches((Polygon p) => p.Points.Count == 3), () => "Could not find some polygons but they don't all have 3 points.");
            Assert.That(polygons, Has.All.With.Matches((Polygon p) => p.Stroke != null || p.Fill != null), () => "All polygons should have their 'Stroke' (and / or 'Fill') property set. Otherwise they are invisible.");
        }

        private void AssertButtonAndCanvasArePresent()
        {
            Assert.That(_canvas, Is.Not.Null, () => "No canvas is found.");
            Assert.That(_button, Is.Not.Null, () => "No draw button is found.");
        }
    }
}
