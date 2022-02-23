using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Guts.Client.Core;
using NUnit.Framework;

namespace Exercise14.Tests
{
    [ExerciseTestFixture("dotNet1", "H05", "Exercise14", @"Exercise14\MainWindow.xaml.cs"), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window;
        private MethodInfo _decimalToBinaryMethod;

        [SetUp]
        public void Setup()
        {
            _window = new MainWindow();

            var windowType = typeof(MainWindow);
            _decimalToBinaryMethod = windowType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(m =>
            {
                if (m.ReturnType != typeof(string)) return false;
                var parameters = m.GetParameters();
                if (parameters.Length != 1) return false;
                return parameters[0].ParameterType == typeof(int);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Close();
        }

        [MonitoredTest("Should have a separate method for conversion"), Order(1)]
        public void _1_ShouldHaveSeperateMethodForConversion()
        {
            AssertHasConversionMethod();
        }

        [MonitoredTest("Should correctly convert integer to binary string representation"), Order(2)]
        public void _2_ShouldCorrectlyConvertIntegerToBinaryStringRepresentation()
        {
            AssertHasConversionMethod();

            var random = new Random();

            AssertConvertion(0, "0");
            AssertConvertion(255, "11111111");

            var someNumber = random.Next(1, 100);
            AssertConvertion(someNumber, Convert.ToString(someNumber, 2));

            someNumber = random.Next(100, 255);
            AssertConvertion(someNumber, Convert.ToString(someNumber, 2));
        }

        private void AssertHasConversionMethod()
        {
            Assert.That(_decimalToBinaryMethod, Is.Not.Null,
                () => "No private method found that accepts an integer as argument and returns a string.");
        }

        private void AssertConvertion(int number, string expected)
        {
            string result = (string)_decimalToBinaryMethod.Invoke(_window, new object[] { number });

            if (expected.Length == 8)
            {
                Assert.That(result, Is.EqualTo(expected),
                    () => $"Converting {number} should result in '{expected}'.");
            }
            else
            {
                Assert.That(result.PadLeft(8, '0'), Is.EqualTo(expected.PadLeft(8, '0')),
                    () => $"Converting {number} should result in '{expected}' or '{expected.PadLeft(8, '0')}'.");
            }
        }
    }
}