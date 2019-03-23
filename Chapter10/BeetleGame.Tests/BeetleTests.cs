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
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BeetleGame.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\Beetle.cs"),
     Apartment(ApartmentState.STA)]
    public class BeetleTests
    {
        private object _beetleObject = null;
        
        private const string _beetleTypeName = "BeetleGame.Beetle";
        private const string _beetleAssembly = "BeetleGame";
        private readonly Type _beetleType = Type.GetType($"{_beetleTypeName}, {_beetleAssembly}");
        private int _beetleSize;
        private int _beetleX;
        private int _beetleY;
        private double _beetleSpeed;

        private Canvas _testCanvas;
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
            _beetleObject = CreateBeetle(_testCanvas, _beetleX, _beetleY, _beetleSize);
            SetPropertyValue(_beetleObject, "Speed", _beetleSpeed);
        }

        [TearDown]
        public void TearDown()
        { }

        [MonitoredTest("Beetle - There should be a class named Beetle"), Order(1)]
        public void _01_ShouldHaveAClassNamedBeetle()
        {
            Assert.That(_beetleType, Is.Not.Null, $"There should be a class named {_beetleTypeName}");
        }

        [MonitoredTest("Beetle - Beetle class should have a parameterized constructor"), Order(2)]
        public void _02_ShouldHaveParameterizedConstructor()
        {
            var constructor = GetConstructor();
            Assert.That(constructor, Is.Not.Null,
                    () => $"{_beetleTypeName} should have a constructor with parameters (Canvas canvas, int x, int y, int size)");
        }

        [MonitoredTest("Beetle - Beetle class should have all required properties"), Order(3)]
        public void _03_ShouldHaveAllProperties()
        {
            var properties = _beetleObject.GetType().GetProperties();
            string[] expectedPropertyNames = { "Speed", "X", "Y", "Size", "Right", "Up", "IsVisible" };
            Type[] expectedPropertyTypes = {typeof(double), typeof(int), typeof(int), typeof(int),
                                            typeof(bool), typeof(bool), typeof(bool)};
            for (int i = 0; i < expectedPropertyNames.Length; i++)
            {
                AssertProperty(properties, expectedPropertyNames[i], expectedPropertyTypes[i],
                           $"{_beetleTypeName} should have a property named ${expectedPropertyNames[i]} of type ${expectedPropertyTypes[i]}.");
            }

        }

        [MonitoredTest("Beetle - Should create a valid Beetle when invoking constructor"), Order(4)]
        public void _04_ShouldCreateValidBeetleWhenInvokingContructor()
        {
            Assert.That(_beetleObject, Is.Not.Null, $"Could not create an instance of class {_beetleTypeName}");
            AssertPropertyValue(_beetleObject, "X", _beetleX);
            AssertPropertyValue(_beetleObject, "Y", _beetleY);
            AssertPropertyValue(_beetleObject, "Size", _beetleSize);
            AssertPropertyValue(_beetleObject, "Up", true);
            AssertPropertyValue(_beetleObject, "Right", true);
            AssertPropertyValue(_beetleObject, "Speed", _beetleSpeed);
        }

        [MonitoredTest("Beetle - Should create a Beetle object with ellipse on its canvas"), Order(5)]
        public void _05_ShouldCreateABeetleWithAnEllipseOnItsCanvas()
        {
            Assert.That(_beetleObject, Is.Not.Null, $"Could not create an instance of class {_beetleTypeName}");
            Assert.That(_testCanvas.Children.Count, Is.GreaterThan(0), $"Beetle should have a Canvas member with an ellipse");
            Assert.That(_testCanvas.Children[0], Is.TypeOf<Ellipse>(), $"Beetle should have a Canvas member with an ellipse");

            // Check correct size and location of ellipse
            var beetleEllipse = (Ellipse)_testCanvas.Children[0];
            Assert.That(beetleEllipse.Width, Is.EqualTo(_beetleSize), $"Ellipse on canvas should have Width ({_beetleSize})");
            Assert.That(beetleEllipse.Height, Is.EqualTo(_beetleSize), $"Ellipse on canvas should have Height ({_beetleSize})");
            var beetleMargin = beetleEllipse.Margin;
            Assert.That(beetleMargin.Left, Is.EqualTo(_beetleX - (_beetleSize / 2)), $"X-Coordinate of ellipse on canvas should be {_beetleX - (_beetleSize / 2)}");
            Assert.That(beetleMargin.Top, Is.EqualTo(_beetleY - (_beetleSize / 2)), $"Y-Coordinate of ellipse on canvas should be {_beetleY - (_beetleSize / 2)}");
        }

        [MonitoredTest("Beetle - Should move up and right without hitting border"), Order(6)]
        public void _06_ShouldMoveUpAndRightWithoutHittingBorder()
        {
            SetPropertyValue(_beetleObject, "Right", true);
            SetPropertyValue(_beetleObject, "Up", true);
            InvokeChangePosition(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, "X", _beetleX + 1);
            AssertPropertyValue(_beetleObject, "Y", _beetleY - 1);
            AssertEllipsePosition(_beetleX + 1, _beetleY - 1);
        }

        [MonitoredTest("Beetle - Should move up and left without hitting border"), Order(7)]
        public void _07_ShouldMoveUpAndLeftWithoutHittingBorder()
        {
            SetPropertyValue(_beetleObject, "Right", false);
            SetPropertyValue(_beetleObject, "Up", true);
            InvokeChangePosition(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, "X", _beetleX - 1);
            AssertPropertyValue(_beetleObject, "Y", _beetleY - 1);
            AssertEllipsePosition(_beetleX - 1, _beetleY - 1);
        }

        [MonitoredTest("Beetle - Should move down and right without hitting border"), Order(8)]
        public void _08_ShouldMoveDownAndLeftWithoutHittingBorder()
        {
            SetPropertyValue(_beetleObject, "Right", false);
            SetPropertyValue(_beetleObject, "Up", false);
            InvokeChangePosition(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, "X", _beetleX - 1);
            AssertPropertyValue(_beetleObject, "Y", _beetleY + 1);
            AssertEllipsePosition(_beetleX - 1, _beetleY + 1);
        }

        [MonitoredTest("Beetle - Should move up and left without hitting border"), Order(9)]
        public void _09_ShouldMoveDownAndRightWithoutHittingBorder()
        {
            SetPropertyValue(_beetleObject, "Right", true);
            SetPropertyValue(_beetleObject, "Up", false);
            InvokeChangePosition(_beetleObject);
            // verify the beetle went in up and right direction
            AssertPropertyValue(_beetleObject, "X", _beetleX + 1);
            AssertPropertyValue(_beetleObject, "Y", _beetleY + 1);
            AssertEllipsePosition(_beetleX + 1, _beetleY + 1);
        }

        [MonitoredTest("Beetle - Should turn down when hitting the upper bound of canvas"), Order(10)]
        public void _10_ShouldTurnDownWhenHittingUpperBoundOfCanvas()
        {
            _beetleY = _beetleSize / 2 + 1; // 1 pixel from top of canvas
            SetPropertyValue(_beetleObject, "Y", _beetleY);
            SetPropertyValue(_beetleObject, "Right", true);
            SetPropertyValue(_beetleObject, "Up", true);      
            InvokeChangePosition(_beetleObject);
            AssertPropertyValue(_beetleObject, "X", _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, "Y", _beetleY - 1); // should go up
            AssertPropertyValue(_beetleObject, "Up", false); // should turn
            AssertPropertyValue(_beetleObject, "Right", true);
        }
        
        [MonitoredTest("Beetle - Should turn up when hitting the lower bound of canvas"), Order(11)]
        public void _11_ShouldTurnUpWhenHittingLowerBoundOfCanvas()
        {
            _beetleY = TestCanvasHeight - (_beetleSize / 2) - 1; // 1 pixel from bottom of canvas
            SetPropertyValue(_beetleObject, "Y", _beetleY);
            SetPropertyValue(_beetleObject, "Right", true);
            SetPropertyValue(_beetleObject, "Up", false);
            InvokeChangePosition(_beetleObject);
            AssertPropertyValue(_beetleObject, "X", _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, "Y", _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, "Up", true); // should turn
            AssertPropertyValue(_beetleObject, "Right", true);
        }

        [MonitoredTest("Beetle - Should turn left when hitting right side of canvas"), Order(12)]
        public void _12_ShouldTurnLeftWhenHittingRightSideOfCanvas()
        {
            _beetleX = TestCanvasWidth - (_beetleSize / 2) - 1; // 1 pixel from right side
            SetPropertyValue(_beetleObject, "X", _beetleX);
            SetPropertyValue(_beetleObject, "Right", true);
            SetPropertyValue(_beetleObject, "Up", false);
            InvokeChangePosition(_beetleObject);
            AssertPropertyValue(_beetleObject, "X", _beetleX + 1); // should go right
            AssertPropertyValue(_beetleObject, "Y", _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, "Right", false);
            AssertPropertyValue(_beetleObject, "Up", false);
        }

        [MonitoredTest("Beetle - Should turn right when hitting left side of canvas"), Order(13)]
        public void _13_ShouldTurnRightWhenHittingLeftSideOfCanvas()
        {
            _beetleX = (_beetleSize / 2) + 1; // 1 pixel from left side
            SetPropertyValue(_beetleObject, "X", _beetleX);
            SetPropertyValue(_beetleObject, "Right", false);
            SetPropertyValue(_beetleObject, "Up", false);
            InvokeChangePosition(_beetleObject);
            AssertPropertyValue(_beetleObject, "X", _beetleX - 1); // should go left
            AssertPropertyValue(_beetleObject, "Y", _beetleY + 1); // should go down
            AssertPropertyValue(_beetleObject, "Right", true);
            AssertPropertyValue(_beetleObject, "Up", false);
        }

        [MonitoredTest("Beetle - a beetle with Speed zero should not move"), Order(14)]
        public void _14_ShouldNotMoveWhenSpeedIsZero()
        {
            _beetleSpeed = 0;
            SetPropertyValue(_beetleObject, "Speed", _beetleSpeed);
            InvokeChangePosition(_beetleObject);
            AssertPropertyValue(_beetleObject, "X", _beetleX); // should not move
            AssertPropertyValue(_beetleObject, "Y", _beetleY); // should not move
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

        private void SetPropertyValue(object obj, string propertyName, object newValue)
        {
            var property = obj.GetType().GetProperty(propertyName);
            property.SetValue(obj, newValue);
        }

        private void InvokeChangePosition(object beetleObject)
        {
            var methodName = "ChangePosition";
            var type = _beetleObject.GetType();
            var method = type.GetRuntimeMethod(methodName, new Type[] { });
            Assert.That(method, Is.Not.Null, $"Should have method {methodName} without arguments and void return type");
            method.Invoke(beetleObject, new object[] { });
        }

        private object CreateBeetle(Canvas canvas, int x, int y, int size)
        {
            object[] parameters = new object[] { _testCanvas, _beetleX, _beetleY, _beetleSize };
            return Activator.CreateInstance(_beetleType, parameters);
        }

        private void AssertProperty(PropertyInfo[] properties, string expectedPropertyName,
                                    Type expectedPropertyType, string message)
        {
            var property = properties.FirstOrDefault(p => p.Name == expectedPropertyName
                                                     && p.PropertyType == expectedPropertyType);
            Assert.That(property, Is.Not.Null, () => message);
        }

        private void AssertPropertyValue(object obj, string propertyName, object expectedValue)
        {
            var property = obj.GetType().GetProperty(propertyName);
            Assert.That(property.GetValue(obj), Is.EqualTo(expectedValue), 
                $"Beetle property {propertyName} has value ({property.GetValue(obj)}) but expected ({expectedValue})");
        }

        private ConstructorInfo GetConstructor()
        {
            return _beetleObject.GetType().GetConstructor(new Type[]
                                {
                                    typeof(Canvas),
                                    typeof(int),
                                    typeof(int),
                                    typeof(int)
                                });
        }
    }
}
