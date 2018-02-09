using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using Guts.Client;
using Guts.Client.TestTools;
using NUnit.Framework;

namespace Exercise05.Tests
{
    [MonitoredTestFixture("dotNet1", 5, 5), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;
        private Canvas _canvas;
        private Button _button;

        [SetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();
            _canvas = _testWindow.GetUIElements<Canvas>().FirstOrDefault();
            _button = _testWindow.GetUIElements<Button>().FirstOrDefault();
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Dispose();
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

            Assert.That(sourceCode, Contains.Substring("new Polygon();"), () => "No code found where an instance of the Polygon class is created.");
            Assert.That(sourceCode, Contains.Substring(".Points.Add("), () => "No code found where points are added to the Polygon instance.");
            Assert.That(sourceCode, Contains.Substring("new Point("), () => "No code found where instances op the Point class are created.");
        }

        [MonitoredTest("Should draw triangles and rectangles on the canvas after the draw button is clicked"), Order(3)]
        public void _3_ShouldDrawTrianglesAndRectanglesOnTheCanvasAfterTheButtonIsClicked()
        {
            AssertButtonAndCanvasArePresent();

            _button.FireClickEvent();

            var allChildren = _canvas.Children.Cast<object>().ToList();
            Assert.That(allChildren, Has.Some.TypeOf<Rectangle>().Or.Some.TypeOf<Polygon>().With.Matches((Polygon p) => p.Points.Count == 4), () => "Could not find any rectangles or polygons with 4 points.");
            Assert.That(allChildren, Has.Some.TypeOf<Polygon>().With.Matches((Polygon p) => p.Points.Count == 3), () => "Could not find any polygons with 3 points.");
        }

        private void AssertButtonAndCanvasArePresent()
        {
            Assert.That(_canvas, Is.Not.Null, () => "No canvas is found.");
            Assert.That(_button, Is.Not.Null, () => "No draw button is found.");
        }
    }
}
