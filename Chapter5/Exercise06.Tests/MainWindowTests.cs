using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;


namespace Exercise06.Tests
{
    [ExerciseTestFixture("dotNet1", "H05", "Exercise06", @"Exercise06\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _testWindow;
        private Canvas _canvas;
        private Button _button;
        private MethodInfo _drawStreetMethod;

        [SetUp]
        public void Setup()
        {
            _testWindow = new MainWindow();
            Grid grid = (Grid)_testWindow.Content;

            _canvas = grid.FindVisualChildren<Canvas>().ToList().FirstOrDefault();
            _button = grid.FindVisualChildren<Button>().ToList().FirstOrDefault();

            var windowType = typeof(MainWindow);
            _drawStreetMethod = windowType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(m => m.Name.ToLower() == "drawstreet");
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

        [MonitoredTest("Should have a 'DrawStreet' method."), Order(2)]
        public void _2_ShouldHaveADrawStreetMethod()
        {
            AssertDrawStreetMethodIsPresent();
        }

        [MonitoredTest("Should draw 4 houses when the button is clicked"), Order(3)]
        public void _3_ShouldDraw4HousesWhendTheButtonIsClicked()
        {
            AssertButtonAndCanvasArePresent();
            AssertDrawStreetMethodIsPresent();

            _button.FireClickEvent();

            var allChildren = _canvas.Children.Cast<object>().ToList();
            Assert.That(allChildren, Has.Exactly(4).TypeOf<Rectangle>(), () => "The canvas should contain exactly 4 rectangles after drawing a street.");
            Assert.That(allChildren, Has.Exactly(12).TypeOf<Line>(), () => "The canvas should contain exactly 12 lines (forming 4 triangles) after drawing a street.");

        }

        private void AssertDrawStreetMethodIsPresent()
        {
            Assert.That(_drawStreetMethod, Is.Not.Null, () => "No method with the name 'DrawStreet' was found");
        }

        private void AssertButtonAndCanvasArePresent()
        {
            Assert.That(_canvas, Is.Not.Null, () => "No canvas is found.");
            Assert.That(_button, Is.Not.Null, () => "No draw button is found.");
        }
    }
}