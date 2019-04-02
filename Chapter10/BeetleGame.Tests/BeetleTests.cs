using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BeetleGame.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\MainWindow.xaml;BeetleGame\MainWindow.xaml.cs;BeetleGame\Beetle.cs"),
     Apartment(ApartmentState.STA)]
    public class BeetleTests
    {
        private object _beetleObject = null;
        
        private int _beetleSize;
        private int _beetleX;
        private int _beetleY;
        private double _beetleSpeed;

        private Canvas _testCanvas;
        private Ellipse _beetleEllipse;
        private const int TestCanvasWidth = 536; // mimic the same width as in the starter file
        private const int TestCanvasHeight = 356; // mimic the same height as in the starter file

        [SetUp]
        public void Setup()
        {
            _testCanvas = new Canvas
            {
                Width = TestCanvasWidth,
                Height = TestCanvasHeight
            };
            _beetleSize = 10;
            _beetleX = 40;
            _beetleY = 35;
            _beetleSpeed = 0.5;
            _beetleObject = BeetleHelper.CreateBeetle(_testCanvas, _beetleX, _beetleY, _beetleSize);
            if (_beetleObject != null)
            {
                BeetleHelper.SetSpeedPropertyValue(_beetleObject, _beetleSpeed);
            }
            _beetleEllipse = (_testCanvas.Children.Count > 0 ? _testCanvas.Children[0] as Ellipse : null);
        }

        [TearDown]
        public void TearDown()
        { }

        [MonitoredTest("Beetle - There should be a class named Beetle"), Order(1)]
        public void _01_ShouldHaveAClassNamedBeetle()
        {
            Assert.That(BeetleHelper.BeetleTypeName, Is.Not.Null, $"There should be a class named {BeetleHelper.BeetleTypeName}");
        }

        [MonitoredTest("Beetle - Beetle class should have a parameterized constructor"), Order(2)]
        public void _02_ShouldHaveParameterizedConstructor()
        {
            Assert.That(_beetleObject, Is.Not.Null,
                    () => $"{BeetleHelper.BeetleTypeName} should have a constructor with parameters (Canvas canvas, int x, int y, int size)");
        }

        [MonitoredTest("Beetle - Beetle class should have all required properties"), Order(3)]
        public void _03_ShouldHaveAllProperties()
        {
            var properties = _beetleObject.GetType().GetProperties();
            string[] expectedPropertyNames =
                { 
                    BeetleHelper.SpeedProperty,
                    BeetleHelper.XProperty,
                    BeetleHelper.YProperty,
                    BeetleHelper.SizeProperty,
                    BeetleHelper.RightProperty,
                    BeetleHelper.UpProperty,
                    BeetleHelper.VisibleProperty
                };
            Type[] expectedPropertyTypes = {typeof(double), typeof(int), typeof(int), typeof(int),
                                            typeof(bool), typeof(bool), typeof(bool)};
            for (int i = 0; i < expectedPropertyNames.Length; i++)
            {
                AssertProperty(properties, expectedPropertyNames[i], expectedPropertyTypes[i],
                           $"{BeetleHelper.BeetleTypeName} should have a property named ${expectedPropertyNames[i]} of type ${expectedPropertyTypes[i]}.");
            }

        }

        [MonitoredTest("Beetle - Should create a valid Beetle when invoking constructor"), Order(4)]
        public void _04_ShouldCreateValidBeetleWhenInvokingContructor()
        {
            Assert.That(_beetleObject, Is.Not.Null, $"Could not create an instance of class {BeetleHelper.BeetleTypeName}");
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX);
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY);
            AssertPropertyValue(_beetleObject, BeetleHelper.SizeProperty, _beetleSize);
            AssertPropertyValue(_beetleObject, BeetleHelper.UpProperty, true);
            AssertPropertyValue(_beetleObject, BeetleHelper.RightProperty, true);
            AssertPropertyValue(_beetleObject, BeetleHelper.SpeedProperty, _beetleSpeed);

        }

        [MonitoredTest("Beetle - Should create a Beetle object with ellipse on its canvas"), Order(5)]
        public void _05_ShouldCreateABeetleWithAnEllipseOnItsCanvas()
        {
            Assert.That(_beetleObject, Is.Not.Null, $"Could not create an instance of class {BeetleHelper.BeetleTypeName}");
            Assert.That(_testCanvas.Children.Count, Is.GreaterThan(0), $"Beetle should have a Canvas member with an ellipse");
            Assert.That(_testCanvas.Children[0], Is.TypeOf<Ellipse>(), $"Beetle should have a Canvas member with an ellipse");
            Assert.That(_beetleEllipse, Is.Not.Null, $"Beetle should have a Canvas member with an ellipse");

            // Check correct size and location of ellipse
            Assert.That(_beetleEllipse.Width, Is.EqualTo(_beetleSize), $"Ellipse on canvas should have Width ({_beetleSize})");
            Assert.That(_beetleEllipse.Height, Is.EqualTo(_beetleSize), $"Ellipse on canvas should have Height ({_beetleSize})");
            var beetleMargin = _beetleEllipse.Margin;
            Assert.That(beetleMargin.Left, Is.EqualTo(_beetleX - (_beetleSize / 2)), $"X-Coordinate of ellipse on canvas should be {_beetleX - (_beetleSize / 2)}");
            Assert.That(beetleMargin.Top, Is.EqualTo(_beetleY - (_beetleSize / 2)), $"Y-Coordinate of ellipse on canvas should be {_beetleY - (_beetleSize / 2)}");
        }

        [MonitoredTest("Beetle - Should move up and right without hitting border"), Order(6)]
        public void _06_ShouldMoveUpAndRightWithoutHittingBorder()
        {
            BeetleHelper.SetRightProperty(_beetleObject, true);
            BeetleHelper.SetUpProperty(_beetleObject, true);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX + 1);
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY - 1);
            AssertEllipsePosition(_beetleX + 1, _beetleY - 1);
        }

        [MonitoredTest("Beetle - Should move up and left without hitting border"), Order(7)]
        public void _07_ShouldMoveUpAndLeftWithoutHittingBorder()
        {
            BeetleHelper.SetRightProperty(_beetleObject, false);
            BeetleHelper.SetUpProperty(_beetleObject, true);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX - 1);
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY - 1);
            AssertEllipsePosition(_beetleX - 1, _beetleY - 1);
        }

        [MonitoredTest("Beetle - Should move down and left without hitting border"), Order(8)]
        public void _08_ShouldMoveDownAndLeftWithoutHittingBorder()
        {
            BeetleHelper.SetRightProperty(_beetleObject, false);
            BeetleHelper.SetUpProperty(_beetleObject, false);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX - 1);
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY + 1);
            AssertEllipsePosition(_beetleX - 1, _beetleY + 1);
        }

        [MonitoredTest("Beetle - Should move down and right without hitting border"), Order(9)]
        public void _09_ShouldMoveDownAndRightWithoutHittingBorder()
        {
            BeetleHelper.SetRightProperty(_beetleObject, true);
            BeetleHelper.SetUpProperty(_beetleObject, false);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX + 1);
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY + 1);
            AssertEllipsePosition(_beetleX + 1, _beetleY + 1);
        }

        [MonitoredTest("Beetle - Should turn down when hitting the upper bound of canvas"), Order(10)]
        public void _10_ShouldTurnDownWhenHittingUpperBoundOfCanvas()
        {
            _beetleY = _beetleSize / 2 + 1; // 1 pixel from top of canvas
            BeetleHelper.SetYProperty(_beetleObject, _beetleY);
            BeetleHelper.SetRightProperty(_beetleObject, true);
            BeetleHelper.SetUpProperty(_beetleObject, true);      
            AssertAndInvokeChangePositionMethod(_beetleObject);
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY - 1); // should go up
            AssertPropertyValue(_beetleObject, BeetleHelper.UpProperty, false); // should turn
            AssertPropertyValue(_beetleObject, BeetleHelper.RightProperty, true);
        }
        
        [MonitoredTest("Beetle - Should turn up when hitting the lower bound of canvas"), Order(11)]
        public void _11_ShouldTurnUpWhenHittingLowerBoundOfCanvas()
        {
            _beetleY = TestCanvasHeight - (_beetleSize / 2) - 1; // 1 pixel from bottom of canvas
            BeetleHelper.SetYProperty(_beetleObject, _beetleY);
            BeetleHelper.SetRightProperty(_beetleObject, true);
            BeetleHelper.SetUpProperty(_beetleObject, false);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, BeetleHelper.UpProperty, true); // should turn
            AssertPropertyValue(_beetleObject, BeetleHelper.RightProperty, true);
        }

        [MonitoredTest("Beetle - Should turn left when hitting right side of canvas"), Order(12)]
        public void _12_ShouldTurnLeftWhenHittingRightSideOfCanvas()
        {
            _beetleX = TestCanvasWidth - (_beetleSize / 2) - 1; // 1 pixel from right side
            BeetleHelper.SetXProperty(_beetleObject, _beetleX);
            BeetleHelper.SetRightProperty(_beetleObject, true);
            BeetleHelper.SetUpProperty(_beetleObject, false);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, BeetleHelper.RightProperty, false);
            AssertPropertyValue(_beetleObject, BeetleHelper.UpProperty, false);
        }

        [MonitoredTest("Beetle - Should turn right when hitting left side of canvas"), Order(13)]
        public void _13_ShouldTurnRightWhenHittingLeftSideOfCanvas()
        {
            _beetleX = (_beetleSize / 2) + 1; // 1 pixel from left side
            BeetleHelper.SetXProperty(_beetleObject, _beetleX);
            BeetleHelper.SetRightProperty(_beetleObject, false);
            BeetleHelper.SetUpProperty(_beetleObject, false);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX - 1); // should go left
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, BeetleHelper.RightProperty, true);
            AssertPropertyValue(_beetleObject, BeetleHelper.UpProperty, false);
        }

        [MonitoredTest("Beetle - Should become invisible when IsVisible property is set"), Order(14)]
        public void _14_ShouldBecomeInvisibleWhenPropertyIsSet()
        {
            BeetleHelper.SetIsVisibleProperty(_beetleObject, true);
            Assert.That(_beetleEllipse.Visibility, Is.EqualTo(Visibility.Visible),
                "Setting IsVisible on Beetle to true should make the ellipse visible");
            BeetleHelper.SetIsVisibleProperty(_beetleObject, false);
            Assert.That(_beetleEllipse.Visibility, Is.EqualTo(Visibility.Hidden),
                "Setting IsVisible on Beetle to false should hide the ellipse");    
        }

        [MonitoredTest("Beetle - a beetle with Speed zero should not move"), Order(15)]
        public void _15_ShouldNotMoveWhenSpeedIsZero()
        {
            _beetleSpeed = 0;
            BeetleHelper.SetSpeedPropertyValue(_beetleObject, _beetleSpeed);
            AssertAndInvokeChangePositionMethod(_beetleObject);
            AssertPropertyValue(_beetleObject, BeetleHelper.XProperty, _beetleX); // should not move
            AssertPropertyValue(_beetleObject, BeetleHelper.YProperty, _beetleY); // should not move
        }

        [MonitoredTest("Beetle - Should compute distance based on current Speed"), Order(16)]
        public void _16_ShouldComputeDistanceBasedOnCurrentSpeed()
        {
            var computeDistanceMethod = AssertComputeDistanceMethod(_beetleObject);
            var startTime = DateTime.Now;
            var endTime = startTime + TimeSpan.FromSeconds(12);
            var elapsedTime = endTime - startTime;
            double expectedDistance = elapsedTime.Seconds * _beetleSpeed / 100;
            double actualDistance = Convert.ToDouble(computeDistanceMethod.Invoke(_beetleObject, new object[] { startTime, endTime}));
            Assert.That(actualDistance, Is.EqualTo(expectedDistance),
                $"computed distance is expected to be ({expectedDistance}) but was ({actualDistance}). " + 
                $"Beetle speed ({_beetleSpeed}), elapsed time in sec ({elapsedTime.Seconds})");
        }

        private void AssertEllipsePosition(int expectedX, int expectedY)
        {
            Assert.That(_testCanvas.Children.Count, Is.EqualTo(1), $"Canvas for Beetle should have 1 ellipse, but found ({_testCanvas.Children.Count})");
            var beetleShape = _testCanvas.Children[0];
            Assert.That(beetleShape, Is.TypeOf<Ellipse>(), $"Shape on canvas should be an ellipse, but found {beetleShape.GetType()}");
            var beetleEllipse = (Ellipse)beetleShape;
            var centerX = beetleEllipse.Margin.Left + (beetleEllipse.Width / 2);
            var centerY = beetleEllipse.Margin.Top + (beetleEllipse.Height / 2);
            Assert.That(centerX, Is.EqualTo(expectedX), $"Center X position of ellipse should be ({expectedX}, but found ({centerX})");
            Assert.That(centerY, Is.EqualTo(expectedY), $"Center Y position of ellipse should be ({expectedY}, but found ({centerY})");
        }

        private MethodInfo AssertComputeDistanceMethod(object beetleObject)
        {
            var methodName = "ComputeDistance";
            var type = _beetleObject.GetType();
            var method = type.GetRuntimeMethod(methodName, new Type[] {typeof(DateTime), typeof(DateTime) });
            Assert.That(method, Is.Not.Null, $"Should have method {methodName} with 2 DateTime arguments and double return type " + 
                " for calculating the Beetle distance based on current Speed");
            return method;
        }

        private MethodInfo AssertChangePositionMethod(object beetleObject)
        {
            var methodName = "ChangePosition";
            var type = _beetleObject.GetType();
            var method = type.GetRuntimeMethod(methodName, new Type[] { });
            Assert.That(method, Is.Not.Null, $"Should have method {methodName} without arguments and void return type for updating the Beetle position");
            return method;
        }

        private void AssertAndInvokeChangePositionMethod(object beetleObject)
        {
            var method = AssertChangePositionMethod(beetleObject);
            method.Invoke(beetleObject, new object[] { });
        }

        private void AssertProperty(PropertyInfo[] properties, string expectedPropertyName,
                                    Type expectedPropertyType, string message)
        {
            var property = properties.FirstOrDefault(p => p.Name == expectedPropertyName
                                                     && p.PropertyType == expectedPropertyType);
            Assert.That(property, Is.Not.Null, () => message);
        }

        private void AssertPropertyValue(object beetleObject, string propertyName, object expectedValue)
        {
            object actualValue = BeetleHelper.GetPropertyValue(beetleObject, propertyName);
            Assert.That(actualValue, Is.EqualTo(expectedValue), 
                $"Beetle property {propertyName} has value ({actualValue}) but expected ({expectedValue})");
        }
    }
}
