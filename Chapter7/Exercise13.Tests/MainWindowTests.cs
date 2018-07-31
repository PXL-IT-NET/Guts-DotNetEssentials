using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using NUnit.Framework;

namespace Exercise13.Tests
{
    [MonitoredTestFixture("dotNet1", 7, 13)]
    [Apartment(ApartmentState.STA)]
    [SetCulture("nl-BE")]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _window;

        private Label _priceLabel;
        private TextBox _priceTextBox;

        private CheckBox _checkBox;

        private Label _btwLabel;
        private TextBox _btwTextBox;

        private Label _totalLabel;
        private TextBox _totalTextBox;

        private Button _button;

        [SetUp]
        public void SetUp()
        {
            _window = new TestWindow<MainWindow>();

            _priceLabel = _window.GetContentControlByPartialContentText<Label>("prijs");
            _btwLabel = _window.GetContentControlByPartialContentText<Label>("btw");
            _totalLabel = _window.GetContentControlByPartialContentText<Label>("totaal");

            var allTextBoxes = _window.GetUIElements<TextBox>();

            _priceTextBox = allTextBoxes.FirstOrDefault(textBox =>
                textBox.Name.ToLower().Contains("price") || textBox.Name.ToLower().Contains("prijs"));

            _btwTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name.ToLower().Contains("btw"));

            _totalTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name.ToLower().Contains("tot"));

            _checkBox = _window.GetUIElements<CheckBox>().FirstOrDefault();

            _button = _window.GetUIElements<Button>().FirstOrDefault();
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Dispose();
        }

        [MonitoredTest("Should have price controls"), Order(1)]
        public void _1_ShouldHavePriceControls()
        {
            Assert.That(_priceLabel, Is.Not.Null, () => "Could not find a Label control with content 'Netto prijs:'");
            Assert.That(_priceTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'price' in its name. Consider using 'priceTextBox' as value for the 'Name' property");
        }

        [MonitoredTest("Should have a checkbox control"), Order(2)]
        public void _2_ShouldHaveACheckBoxControl()
        {
            Assert.That(_checkBox, Is.Not.Null, () => "Could not find a CheckBox control");
            Assert.That(_checkBox.Content, Contains.Substring("Verlaagd tarief").IgnoreCase, () => "The 'Content' of the checkbox should be 'Verlaagd tarief'");
        }

        [MonitoredTest("Should have BTW controls"), Order(3)]
        public void _3_ShouldHaveBtwControls()
        {
            Assert.That(_btwLabel, Is.Not.Null, () => "Could not find a Label control with content 'BTW:'");
            Assert.That(_btwTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'btw' in its name. Consider using 'btwTextBox' as value for the 'Name' property");
        }

        [MonitoredTest("Should have total controls"), Order(4)]
        public void _4_ShouldHaveTotalControls()
        {
            Assert.That(_totalLabel, Is.Not.Null, () => "Could not find a Label control with content 'Totaal:'");
            Assert.That(_totalTextBox, Is.Not.Null, () => "Could not find a TextBox control that has the text 'tot' in its name. Consider using 'totalTextBox' as value for the 'Name' property");
        }

        [MonitoredTest("Should have a button"), Order(5)]
        public void _5_ShouldHaveAButton()
        {
            Assert.That(_button, Is.Not.Null, () => "Could not find a Button control");
            Assert.That(_button.Content, Contains.Substring("bereken").IgnoreCase, () => "The 'Content' of the button should be 'Bereken'");
        }

        [MonitoredTest("Should display btw and total as readonly textboxes"), Order(6)]
        public void _6_ShouldDisplayBtwAndTotalAsReadOnlyTextBoxes()
        {
            AssertAllControlsArePresent();

            Assert.That(_btwTextBox.IsEnabled, Is.False, () => "The BTW TextBox is not readonly. Tip: 'IsEnabled' property.");
            Assert.That(_totalTextBox.IsEnabled, Is.False, () => "The total TextBox is not readonly. Tip: 'IsEnabled' property.");
        }

        [MonitoredTest("Should calculate Btw at 21% when checkbox is unchecked"), Order(7)]
        public void _7_ShouldCalculateBtwAtRate21WhenCheckBoxIsUnchecked()
        {
            AssertAllControlsArePresent();

            var netPrice = "50";
            _priceTextBox.Text = netPrice;
            _checkBox.IsChecked = false;

            _button.FireClickEvent();

            Assert.That(_btwTextBox.Text, Is.EqualTo("10,5"), () => $"Btw for net price of '{netPrice}' is not correct");
            Assert.That(_totalTextBox.Text, Is.EqualTo("60,5"), () => $"Total for net price of '{netPrice}' is not correct");
        }

        [MonitoredTest("Should calculate Btw at 6% when checkbox is checked"), Order(8)]
        public void _8_ShouldCalculateBtwAtRate6WhenCheckBoxIsChecked()
        {
            AssertAllControlsArePresent();

            var netPrice = "200";
            _priceTextBox.Text = netPrice;
            _checkBox.IsChecked = true;

            _button.FireClickEvent();

            Assert.That(_btwTextBox.Text, Is.EqualTo("12"), () => $"Btw for net price of '{netPrice}' is not correct");
            Assert.That(_totalTextBox.Text, Is.EqualTo("212"), () => $"Total for net price of '{netPrice}' is not correct");
        }

        private void AssertAllControlsArePresent()
        {
            _1_ShouldHavePriceControls();
            _2_ShouldHaveACheckBoxControl();
            _3_ShouldHaveBtwControls();
            _4_ShouldHaveTotalControls();
            _5_ShouldHaveAButton();
        }
    }
}
