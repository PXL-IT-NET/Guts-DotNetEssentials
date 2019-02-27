using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
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

namespace Exercise08.Tests
{
    [ExerciseTestFixture("dotNet1", "H12", "Exercise08", @"Exercise08\MainWindow.xaml;Exercise08\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _mainWindow;
        private TextBlock _errorTextBlock;
        private readonly string _errorTextBlockName = "errorTextBlock";
        private TextBlock _areaTextBlock;
        private readonly string _areaTextBlockName = "areaTextBlock";
        private TextBox[] _sideTextBoxes;
        private Button _calculateButton;
        private MethodInfo _maxFunction;
        private readonly string _maxFunctionName = "Max";

        [SetUp]
        public void SetUp()
        {
            _mainWindow = new TestWindow<MainWindow>();
            _errorTextBlock = _mainWindow.GetUIElements<TextBlock>().FirstOrDefault((tb) => tb.Name == _errorTextBlockName);
            _areaTextBlock = _mainWindow.GetUIElements<TextBlock>().FirstOrDefault((tb) => tb.Name == _areaTextBlockName);
            _sideTextBoxes = _mainWindow.GetUIElements<TextBox>().ToArray();
            _calculateButton = _mainWindow.GetUIElements<Button>().FirstOrDefault();

            var windowType = typeof(MainWindow);
            _maxFunction = windowType.GetMethods(BindingFlags.NonPublic |
                                                 BindingFlags.Public |
                                                 BindingFlags.Instance).FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != 3) return false;
                if (m.ReturnType != typeof(int)) return false;
                return true;
            });
        }

        [TearDown]
        public void TearDown()
        {
            _mainWindow?.Dispose();
        }

        [MonitoredTest("Should have two TextBlocks for area and error"), Order(1)]
        public void _1_ShouldHaveTwoTextBlocksForAreaAndError()
        {
            AssertHaveTwoTextBlocksProperlyInitialized();
        }

        [MonitoredTest("Should have a Max function with 3 parameters"), Order(2)]
        public void _2_ShouldHaveAMaxFunction()
        {
            AssertHasMaxFunction();
        }

        [MonitoredTest("Max function should compute the greatest of 3 int parameters"), Order(3)]
        public void _3_MaxShouldWorkProperly()
        {
            AssertHasMaxFunction();

            AssertMaxInvoke(10, 10, 2, 3);
            AssertMaxInvoke(100, 10, 100, 3);
            AssertMaxInvoke(200, 10, 100, 200);
            AssertMaxInvoke(0, 0, 0, 0);
        }

        [MonitoredTest("Should have 3 TextBoxes for entering sides"), Order(4)]
        public void _4_ShouldHaveThreeTextBoxesForSideInput()
        {
            AssertHasTextBoxes();
        }

        [MonitoredTest("Sides that do not form a triangle should generate an error"), Order(5)]
        public void _5_SidesThatDoNotFormTriangleShouldGenerateErrorMessages()
        {
            AssertHasTextBoxes();
            Assert.That(_calculateButton, Is.Not.Null); // is provided in template, should be ok

            AssertErrorMessageWithIllegalSides("10", "3", "4");
            AssertErrorMessageWithIllegalSides("3", "4", "10");
            AssertErrorMessageWithIllegalSides("3", "10", "4");
        }

        [MonitoredTest("Sides that form a triangle should produce a correct area"), Order(6)]
        public void _6_SidesThatFormTriangleShouldProduceCorrectArea()
        {
            AssertHasTextBoxes();
            Assert.That(_calculateButton, Is.Not.Null); // is provided in template, should be ok

            AssertAreaWithLegalSides(6, "3", "4", "5");
            AssertAreaWithLegalSides(44.039, "9", "10", "12");
        }

        [MonitoredTest("Mixing illegal sides and legal sides should clear TextBlocks correctly"), Order(7)]
        public void _7_MixingIllegalSidesWithLegalSidesShouldClearTextBlocksCorrectly()
        {
            AssertHasTextBoxes();
            Assert.That(_calculateButton, Is.Not.Null); // is provided in template, should be ok

            AssertAreaWithLegalSides(6, "3", "4", "5");
            AssertErrorMessageWithIllegalSides("3", "10", "4");
            AssertAreaWithLegalSides(44.039, "9", "10", "12");
        }

        private void AssertAreaWithLegalSides(double expected, string sideA, string sideB, string sideC)
        {
            _sideTextBoxes[0].Text = sideA;
            _sideTextBoxes[1].Text = sideB;
            _sideTextBoxes[2].Text = sideC;

            _calculateButton.FireClickEvent();

            Assert.That(_errorTextBlock.Text, Is.EqualTo(string.Empty),
                () => "Sides that form a triangle should not give an error message");

            double area = double.Parse(_areaTextBlock.Text);
            Assert.That(area, Is.EqualTo(expected),
                () => "Sides that form a triangle should produce an area rounded to 3 decimal digits");
        }

        private void AssertErrorMessageWithIllegalSides(string sideA, string sideB, string sideC)
        {
            _sideTextBoxes[0].Text = sideA;
            _sideTextBoxes[1].Text = sideB;
            _sideTextBoxes[2].Text = sideC;

            _calculateButton.FireClickEvent();

            Assert.That(_errorTextBlock.Text, Is.Not.EqualTo(string.Empty),
                () => "Sides that don't form a triangle should give an error message");

            Assert.That(_areaTextBlock.Text, Is.EqualTo(string.Empty),
                () => "Sides that don't form a triangle should not produce an area");
        }

        private void AssertHasTextBoxes()
        {
            Assert.That(_sideTextBoxes, Is.Not.Null);
            Assert.That(_sideTextBoxes.Length, Is.EqualTo(3),
                () => "There should be 3 TextBoxes for entering sides.");
        }

        private void AssertMaxInvoke(int expected, int a, int b, int c)
        {
            object[] parameters = new object[] { a, b, c };
            object result = _maxFunction.Invoke(_mainWindow.Window, parameters);
            Assert.That(result, Is.Not.Null);
            Assert.That((int)result, Is.EqualTo(expected));
        }

        private void AssertHasMaxFunction()
        {
            Assert.That(_maxFunction, Is.Not.Null,
                            () => $"No function found called {_maxFunctionName}");

            var parameters = _maxFunction.GetParameters();
            bool predicate = parameters[0].ParameterType == typeof(int) &&
                             parameters[1].ParameterType == typeof(int) &&
                             parameters[2].ParameterType == typeof(int);
            Assert.That(predicate, Is.True,
                () => $"{_maxFunctionName} should have 3 int parameters");
        }

        private void AssertHaveTwoTextBlocksProperlyInitialized()
        {
            Assert.That(_areaTextBlock, Is.Not.Null,
                () => $"No TextBlock found with name = {_areaTextBlockName}");
            Assert.That(_errorTextBlock, Is.Not.Null,
                () => $"No TextBlock found with name = {_errorTextBlockName}");
            Assert.That(_areaTextBlock.Text, Is.EqualTo(string.Empty),
                () => $"{_areaTextBlockName}.Text should be empty");
            Assert.That(_errorTextBlock.Text, Is.EqualTo(string.Empty),
                () => $"{_errorTextBlockName}.Text should be empty");
        }
    }
}
