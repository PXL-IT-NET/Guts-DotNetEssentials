using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using Guts.Client;
using Guts.Client.TestTools.WPF;
using NUnit.Framework;

namespace Exercise07.Tests
{
    [MonitoredTestFixture("dotNet1", 7, 7)]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _window;

        private Button[] _numberButtons;
        private Button _plusOperatorButton;
        private Button _minusOperatorButton;
        private Button _evaluateOperatorButton;
        private Button _clearButton;
        private TextBlock _displayTextBlock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _window = new TestWindow<MainWindow>();

            _numberButtons = new Button[10];
            for (int digit = 0; digit <= 9; digit++)
            {
                _numberButtons[digit] = _window.GetContentControlByPartialContentText<Button>(digit.ToString());
            }
            _plusOperatorButton = _window.GetContentControlByPartialContentText<Button>("+");
            _minusOperatorButton = _window.GetContentControlByPartialContentText<Button>("-");
            _evaluateOperatorButton = _window.GetContentControlByPartialContentText<Button>("=");

            _clearButton = _window.GetContentControlByPartialContentText<Button>("clear");

            _displayTextBlock = _window.GetUIElements<TextBlock>()
                .FirstOrDefault(textBlock => !string.IsNullOrEmpty(textBlock.Name)); //filter on name property because the buttons internally also contain TextBlocks
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _window?.Dispose();
        }

        [MonitoredTest("Should have all controls"), Order(1)]
        public void _1_ShouldHaveAllControls()
        {
            for (var digit = 0; digit < _numberButtons.Length; digit++)
            {
                var numberButton = _numberButtons[digit];
                var message = $"Number Button with text '{digit}' is missing";
                Assert.That(numberButton, Is.Not.Null, () => message);
            }

            Assert.That(_minusOperatorButton, Is.Not.Null, () => "Operator Button with text '-' is missing");
            Assert.That(_plusOperatorButton, Is.Not.Null, () => "Operator Button with text '+' is missing");
            Assert.That(_evaluateOperatorButton, Is.Not.Null, () => "Operator Button with text '=' is missing");

            Assert.That(_clearButton, Is.Not.Null, () => "A Button with text 'Clear' is missing");

            Assert.That(_displayTextBlock, Is.Not.Null, () => "A display TextBlock is missing");
        }

        [MonitoredTest("Should display numbers when clicked"), Order(2)]
        public void _2_ShouldDisplayNubmersWhenClicked()
        {
            Assert.That(_displayTextBlock.Text, Is.EqualTo("0"), () => "At startup the display text should be equal to 0");

            var expectedDisplayBuilder = new StringBuilder();
            for (var digit = 9; digit >= 0; digit--)
            {
                var numberButton = _numberButtons[digit];
                numberButton.FireClickEvent();

                expectedDisplayBuilder.Append(digit);

                var message = $"Display text is not corrcect after clicking on '{digit}'";
                Assert.That(_displayTextBlock.Text, Is.EqualTo(expectedDisplayBuilder.ToString()), () => message);
            }
        }

        [MonitoredTest("Should reset display after clearing"), Order(3)]
        public void _3_ShouldResetDisplayAfterClearing()
        {
            _clearButton.FireClickEvent();
            Assert.That(_displayTextBlock.Text, Is.EqualTo("0"), "After clicking the clear button, the display should show 0");
        }

        [MonitoredTest("Should add correctly"), Order(4)]
        public void _4_ShouldAddCorrectly()
        {
            AssertCalculation("3", "1", "+", "2", "=");
            AssertCalculation("2", "1", "+", "2");
            AssertCalculation("1", "1", "+");
            AssertCalculation("67", "1", "2", "+", "5", "5", "+");
        }

        [MonitoredTest("Should substract correctly"), Order(5)]
        public void _5_ShouldSubtractCorrectly()
        {
            AssertCalculation("5", "8", "-", "3", "=");
            AssertCalculation("5", "1", "0", "-", "5", "+");
            AssertCalculation("-1", "1", "-", "2", "=");
        }

        [MonitoredTest("Should start new calculation after equal sign"), Order(6)]
        public void _6_ShouldStartNewCalculationAfterEqualSign()
        {
            AssertCalculation("8", "1", "+", "2", "=", "5", "+", "3", "=");
        }

        [MonitoredTest("Should do complex calculation"), Order(7)]
        public void _7_ShouldDoComplexCalculation()
        {
            AssertCalculation("280", "1", "0", "-", "5", "0", "+", "3", "0", "0", "+", "2", "0", "=");
        }

        private void AssertCalculation(string expectedResult, params string[] clicks)
        {
            _clearButton.FireClickEvent();

            var calculationBuilder = new StringBuilder();
            foreach (var click in clicks)
            {
                switch (click)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        int digit = int.Parse(click);
                        _numberButtons[digit].FireClickEvent();
                        break;
                    case "+":
                        _plusOperatorButton.FireClickEvent();
                        break;
                    case "-":
                        _minusOperatorButton.FireClickEvent();
                        break;
                    case "=":
                        _evaluateOperatorButton.FireClickEvent();
                        break;
                }
                calculationBuilder.Append(click);
            }
            Assert.That(_displayTextBlock.Text, Is.EqualTo(expectedResult),
                () => $"Clicking '{calculationBuilder}' should result in '{expectedResult}' being displayed");
        }
    }
}
