using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise03.Tests
{
    [ExerciseTestFixture("dotNet1", "H06", "Exercise03", @"Exercise03\MainWindow.xaml;Exercise03\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private const string RandomTextBoxName = "randomTextBox";
        private const string SumTextBoxName = "sumTextBox";
        private const string AverageTextBoxName = "averageTextBox";

        private MainWindow _testWindow;
        private Button _theButton;
        private TextBox _randomTextBox;
        private TextBox _sumTextBox;
        private TextBox _averageTextBox;

        [SetUp]
        public void Setup()
        {
            _testWindow = new MainWindow();
            Grid grid = (Grid)_testWindow.Content;

            _theButton = grid.FindVisualChildren<Button>().ToList().FirstOrDefault();

            var allTextBoxes = grid.FindVisualChildren<TextBox>().ToList();
            _randomTextBox = allTextBoxes.Where(field => field.Name.ToLower().Contains("random")).FirstOrDefault();
            _sumTextBox = allTextBoxes.Where(field => field.Name.ToLower().Contains("sum")).FirstOrDefault();
            _averageTextBox = allTextBoxes.Where(field => field.Name.ToLower().Contains("average")).FirstOrDefault();
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Close();
        }

        [MonitoredTest("Should have 3 disabled TextBoxes and a button"), Order(1)]
        public void _1_ShouldHave3DisabledTextBoxesAndAButton()
        {
            Assert.IsNotNull(_randomTextBox, $"Could not find a TextBox with a name like '{RandomTextBoxName}'");
            Assert.That(_randomTextBox.IsEnabled, Is.False, () => "The random TextBox is enabled. Please set the 'IsEnabled' property to 'False'");

            Assert.IsNotNull(_sumTextBox, $"Could not find a TextBox with the name like '{SumTextBoxName}'");
            Assert.That(_sumTextBox.IsEnabled, Is.False, () => "The sum TextBox is enabled. Please set the 'IsEnabled' property to 'False'");

            Assert.IsNotNull(_averageTextBox, $"Could not find a TextBox with the name like '{AverageTextBoxName}'");
            Assert.That(_averageTextBox.IsEnabled, Is.False, () => "The average TextBox is enabled. Please set the 'IsEnabled' property to 'False'");

            Assert.IsNotNull(_theButton, "Could not find a Button");
        }

        [MonitoredTest("Should start with empty TextBoxes"), Order(2)]
        public void _2_ShouldStartWithEmptyTextBoxes()
        {
            Assert.That(_randomTextBox.Text, Is.Empty.Or.EqualTo("0"), () => "At first the random TextBox should be empty or contain the number zero");
            Assert.That(_sumTextBox.Text, Is.Empty.Or.EqualTo("0"), () => "At first the sum TextBox should be empty or contain the number zero");
            Assert.That(_averageTextBox.Text, Is.Empty.Or.EqualTo("0"), () => "At first the average TextBox should be empty or contain the number zero");
        }

        [MonitoredTest("Should show the same random number between 200 and 400 in all TextBoxes after one click"), Order(3)]
        public void _3_ShouldShowTheSameRandomNumberInAllTextBoxesAfterOneClick()
        {
            _theButton.FireClickEvent();

            Assert.That(double.TryParse(_randomTextBox.Text, out double firstNumber), Is.True, () => "After one click the random TextBox should contain a number");
            Assert.That(firstNumber, Is.InRange(200, 400), () => "The number in the random TextBox should be between 200 and 400");
            Assert.That(_sumTextBox.Text, Is.EqualTo(_randomTextBox.Text), () => "After one click the sum TextBox should contain the same number as the random TextBox");
            Assert.That(_averageTextBox.Text, Is.EqualTo(_randomTextBox.Text), () => "After one click the average TextBox should contain the same number as the random TextBox");
        }

        [MonitoredTest("Should show sum and average after 2 clicks"), Order(4)]
        public void _4_ShouldShowSumAndAverageAfter2Clicks()
        {
            _theButton.FireClickEvent();
            double firstNumber = double.Parse(_randomTextBox.Text);

            _theButton.FireClickEvent();

            Assert.That(double.TryParse(_randomTextBox.Text, out double secondNumber), Is.True, () => "After the second click the random TextBox should contain a number");
            Assert.That(secondNumber, Is.InRange(200, 400), () => "The number in the random TextBox should be between 200 and 400");
            Assert.That(secondNumber, Is.Not.EqualTo(firstNumber), () => "After the second click the random TextBox should contain a different number");
            Assert.That(_sumTextBox.Text, Is.EqualTo((firstNumber + secondNumber).ToString()), () => "After the second click the sum TextBox should contain the sum");
            Assert.That(_averageTextBox.Text, Is.EqualTo(((firstNumber + secondNumber) / 2).ToString()), () => "After the second click the average TextBox should contain the average");
        }

        [MonitoredTest("Should convert to an average of 300"), Order(5)]
        public void _5_ShouldConvertToAnAverageOf300()
        {
            var numberOfClicks = 100;
            for (int i = 0; i < numberOfClicks; i++)
            {
                _theButton.FireClickEvent();

                double number = double.Parse(_randomTextBox.Text);
                Assert.That(number, Is.InRange(200, 400), () => "The number in the random TextBox should always be between 200 and 400");
            }

            Assert.That(double.TryParse(_sumTextBox.Text, out double sum), Is.True, () => $"After {numberOfClicks} clicks the sum TextBox should contain a number");
            Assert.That(sum, Is.GreaterThan(numberOfClicks * 200), () => $"After {numberOfClicks} clicks the sum TextBox should contain a number greather than {numberOfClicks} * 200");

            Assert.That(double.TryParse(_averageTextBox.Text, out double average), Is.True, () => $"After {numberOfClicks} clicks the average TextBox should contain a number");
            Assert.That(average, Is.EqualTo(300.0).Within(30.0), () => $"After {numberOfClicks} clicks the average TextBox should contain a number close to 300");
        }
    }
}
