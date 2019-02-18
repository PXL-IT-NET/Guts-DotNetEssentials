using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using Guts.Client.Shared;
using NUnit.Framework;

namespace Exercise07.Tests
{
    [ExerciseTestFixture("dotNet1", "H13", "Exercise07", @"Exercise07\MainWindow.xaml;Exercise07\MainWindow.xaml.cs"), 
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _window;
        private TextBox _monthNumberTextBox;
        private TextBox _monthNameTextBox;
        private Button _lookupButton;

        [SetUp]
        public void Setup()
        {
            _window = new TestWindow<MainWindow>();
            var allTextBoxes = _window.GetPrivateFields<TextBox>();
            _monthNumberTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name == "monthNumberTextBox");
            _monthNameTextBox = allTextBoxes.FirstOrDefault(textBox => textBox.Name == "monthNameTextBox");
            _lookupButton = _window.GetPrivateField<Button>(field => field.Name == "lookupButton");
        }

        [TearDown]
        public void TearDown()
        {
            _window.Dispose();
        }

        [MonitoredTest("Should have 2 TextBoxes and a Button"), Order(1)]
        public void _1_ShouldHave2TextBoxesAndAButton()
        {
            AssertHasAllControls();
        }

        [MonitoredTest("Should not have a ListBox"), Order(2)]
        public void _2_ShouldNotHaveAListBox()
        {
            var listBox = _window.GetPrivateField<ListBox>();
            Assert.That(listBox, Is.Null);
        }

        [MonitoredTest("Should use a generic List"), Order(3)]
        public void _3_ShouldUseAGenericList()
        {
            var listOfMonths = _window.GetPrivateField<List<string>>();

            Assert.That(listOfMonths, Is.Not.Null, 
                () => "Cannot find a declaration of a field (instance variable) in the class that is a generic list that can contain names of months.");
        }

        [MonitoredTest("Should lookup months correctly"), Order(4)]
        public void _4_ShouldLookUpMonthsCorrectly()
        {
            AssertMonthLookup(1, "January", "Januari");
            AssertMonthLookup(2, "February", "Februari");
            AssertMonthLookup(3, "March", "Maart");
            AssertMonthLookup(4, "April", "April");
            AssertMonthLookup(5, "May", "Mei");
            AssertMonthLookup(6, "June", "Juni");
            AssertMonthLookup(7, "July", "Juli");
            AssertMonthLookup(8, "August", "Augustus");
            AssertMonthLookup(9, "September", "September");
            AssertMonthLookup(10, "October", "Oktober");
            AssertMonthLookup(11, "November", "November");
            AssertMonthLookup(12, "December", "December");
        }

        private void AssertMonthLookup(int monthNumber, string expectedMonthName, string alternativeMonthName)
        {
            _monthNumberTextBox.Text = monthNumber.ToString();
            _lookupButton.FireClickEvent();
            Assert.That(_monthNameTextBox.Text, 
                Is.EqualTo(expectedMonthName).IgnoreCase.Or.EqualTo(alternativeMonthName).IgnoreCase, 
                () => $"Looking up month number {monthNumber}, results in '{_monthNameTextBox.Text}' while '{expectedMonthName}' is expected.");
        }

        private void AssertHasAllControls()
        {
            Assert.That(_monthNumberTextBox, Is.Not.Null, () => "No TextBox with the name 'monthNumberTextBox' can be found.");
            Assert.That(_monthNameTextBox, Is.Not.Null, () => "No TextBox with the name 'monthNameTextBox' can be found.");
            Assert.That(_lookupButton, Is.Not.Null, () => "No Button with the name 'lookupButton' can be found.");
        }
    }
}
