using Guts.Client;
using Guts.Client.TestTools;
using Guts.Client.TestTools.WPF;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Exercise04.Tests
{
    [MonitoredTestFixture("dotNet1", 8, 4), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;
        private Canvas _canvas;

        private string _mainWindowSourceCode;

        [SetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();

            _mainWindowSourceCode = Solution.Current.GetFileContent(@"Exercise04\MainWindow.xaml.cs");
            _mainWindowSourceCode = CodeCleaner.StripComments(_mainWindowSourceCode);
            _canvas = _testWindow.GetPrivateField<Canvas>();
            
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Dispose();
        }

        [MonitoredTest("Should have a private method DrawRectangle"), Order(1)]
        public void _1_ShouldHaveAPrivateMethodDrawRectangle()
        {
            bool hasDrawRectangeleMethod  =_testWindow.Window.HasPrivateMethod(method =>
                method.Name.ToLower().Contains("drawrectangle") && method.ReturnType == typeof(void));

            Assert.That(hasDrawRectangeleMethod, Is.True,
                () => "Could not find a method 'DrawRectangle' with return type 'void'.");
        }

        [MonitoredTest("Should make use of the Rectangle class"), Order(2)]
        public void _2_ShouldMakeUseOfRectangleClass()
        {
            Assert.That(_mainWindowSourceCode, Contains.Substring("new Rectangle();"),
                () => "No code found where an instance of the Rectangle class is created.");
        }

        [MonitoredTest("Should set properties of the Rectangle class instance"), Order(3)]
        public void _3_ShouldSetPropertiesOfRectangleClassInstance()
        {
            // rectangle weglaten
            Assert.That(_mainWindowSourceCode, Contains.Substring("rectangle.Width = "),
                () => "No code found where the Width property of the rectangle is set.");
            Assert.That(_mainWindowSourceCode, Contains.Substring("rectangle.Height = "),
                () => "No code found where the Height property of the rectangle is set.");
            Assert.That(_mainWindowSourceCode, Contains.Substring("rectangle.Margin = "),
                () => "No code found where the Margin property of the rectangle is set.");
            Assert.That(_mainWindowSourceCode, Contains.Substring("rectangle.Stroke = "),
                () => "No code found where the Stroke property of the rectangle is set.");
        }
        
        [MonitoredTest("Should create a staircase with Rectangles at startup"), Order(4)]
        public void _4_ShouldCreateStaircase()
        {
            var rectangles = _canvas.FindVisualChildren<Rectangle>().ToList();
            var rectanglesGroupedByX = rectangles.GroupBy(rectangle => rectangle.Margin.Left).ToList();
            var rectanglesGroupedByY = rectangles.GroupBy(rectangle => rectangle.Margin.Top).ToList();

            Assert.That(rectangles.Count, Is.EqualTo(21),
                () => $"The stair should consist out of 21 Rectangles, but {rectangles.Count} were found.");

            Assert.That(rectanglesGroupedByX.Count, Is.EqualTo(6),
                () => $"The stair should have 6 columns of rectangles (same x-coordinate), but {rectanglesGroupedByX.Count} were found.");

            Assert.That(rectanglesGroupedByY.Count, Is.EqualTo(6),
                () => $"The stair should have 6 rows of rectangles (same y-coordinate), but {rectanglesGroupedByY.Count} were found.");


            for (int i = 1; i <= 6; i++)
            {
                var expectedNumberOfRectangles = i;
                Assert.That(rectanglesGroupedByX.Any(col => col.Count() == expectedNumberOfRectangles), Is.True, 
                    () => $"The stair should contain one column with {expectedNumberOfRectangles} rectangles, but such a column was not found.");

                Assert.That(rectanglesGroupedByY.Any(row => row.Count() == expectedNumberOfRectangles), Is.True,
                    () => $"The stair should contain one row with {expectedNumberOfRectangles} rectangles, but such a row was not found.");
            }
        }
    }
}
