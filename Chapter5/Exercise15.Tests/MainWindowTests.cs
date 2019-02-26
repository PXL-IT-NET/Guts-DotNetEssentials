using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;

namespace Exercise15.Tests
{
    [ExerciseTestFixture("dotNet1", "H05", "Exercise15", @"Exercise15\MainWindow.xaml.cs"), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window;
        private MethodInfo _convertSecondsToHoursMinutesSecondsMethod;

        [SetUp]
        public void Setup()
        {
            _window = new MainWindow();

            var windowType = typeof(MainWindow);
            _convertSecondsToHoursMinutesSecondsMethod = windowType.GetMethods(BindingFlags.NonPublic |
                                                                               BindingFlags.Public |
                                                                               BindingFlags.Instance).FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != 4) return false;
                bool predicate = parameters[0].ParameterType == typeof(int) &&
                       parameters[1].ParameterType.Name == "Int32&" && parameters[1].IsOut &&
                       parameters[2].ParameterType.Name == "Int32&" && parameters[2].IsOut &&
                       parameters[3].ParameterType.Name == "Int32&" && parameters[3].IsOut &&
                       m.ReturnType == typeof(void);
                return predicate;
            });
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Close();
        }

        [MonitoredTest("Should have a separate method for conversion"), Order(1)]
        public void _1_ShouldHaveSeparateMethodForConversion()
        {
            AssertHasConversionMethod();
        }

        [MonitoredTest("Should correctly convert seconds to hours, minutes and seconds"), Order(2)]
        public void _2_ShouldCorrectlyConvertSecondsToHoursMinutesSeconds()
        {
            AssertHasConversionMethod();

            AssertConversion(3662, 1, 1, 2);
            AssertConversion(0, 0, 0, 0);
            AssertConversion(1, 0, 0, 1);
            AssertConversion(60, 0, 1, 0);
            AssertConversion(61, 0, 1, 1);
            AssertConversion(3600, 1, 0, 0);
        }

        private void AssertHasConversionMethod()
        {
            Assert.That(_convertSecondsToHoursMinutesSecondsMethod, Is.Not.Null,
                () => "No private method found that accepts 4 integers");
        }

        private void AssertConversion(int inputSeconds, int expectedHours, int expectedMinutes, int expectedSeconds)
        {
            object[] parameters = new object[] { inputSeconds, null, null, null };


            _convertSecondsToHoursMinutesSecondsMethod.Invoke(_window, parameters );

            int outHours = (int)parameters[1];
            int outMinutes = (int)parameters[2];
            int outSeconds = (int)parameters[3];

            Assert.That(outHours, Is.EqualTo(expectedHours),
                        () => $"Converting {inputSeconds} should result into {expectedHours} hours, {expectedMinutes} minutes, {expectedSeconds} seconds");
            Assert.That(outMinutes, Is.EqualTo(expectedMinutes),
                () => $"Converting {inputSeconds} should result into {expectedHours} hours, {expectedMinutes} minutes, {expectedSeconds} seconds");
            Assert.That(outSeconds, Is.EqualTo(expectedSeconds),
                () => $"Converting {inputSeconds} should result into {expectedHours} hours, {expectedMinutes} minutes, {expectedSeconds} seconds");
        }
    }
}
