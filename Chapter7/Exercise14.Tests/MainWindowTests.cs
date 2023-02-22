using System.Linq;
using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using NUnit.Framework;

namespace Exercise14.Tests
{
    [ExerciseTestFixture("dotNet1", "H07", "Exercise14", @"Exercise14\MainWindow.xaml;Exercise14\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window;

        private GroupBox _genderGroupBox;

        private GroupBox _ageGroupBox;

        private Button _button;

        [SetUp]
        public void SetUp()
        {
            _window = new MainWindow();

            var allGroupBoxes = _window.GetAllPrivateFieldValues<GroupBox>();

            _genderGroupBox =
                allGroupBoxes.FirstOrDefault(groupBox => ((string) groupBox.Header).ToLower().Contains("geslacht"));

            _ageGroupBox =
                allGroupBoxes.FirstOrDefault(groupBox => ((string)groupBox.Header).ToLower().Contains("leeftijd"));

            _button = _window.GetPrivateFieldValue<Button>();
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Close();
        }

        [MonitoredTest("Should have name and firstname controls"), Order(1)]
        public void _1_ShouldHaveNameAndFirstNameControls()
        {
            var lastnameLabel = _window.GetPrivateFieldValueByName<Label>("lastnameLabel");
            var firstnameLabel = _window.GetPrivateFieldValueByName<Label>("firstnameLabel");

            var allTextBoxes = _window.GetAllPrivateFieldValues<TextBox>();

            var lastnameTextBox = allTextBoxes.ElementAt(0);
            var firstnameTextBox = allTextBoxes.ElementAt(1);

            Assert.That(lastnameLabel, Is.Not.Null, () => "Could not find a Label control with content 'Naam:'");
            Assert.That(firstnameLabel, Is.Not.Null, () => "Could not find a Label control with content 'Voornaam:'");

            Assert.That(lastnameTextBox, Is.Not.Null, () => "Could not find a TextBox control for lastname");
            Assert.That(firstnameTextBox, Is.Not.Null, () => "Could not find a TextBox control for firstname");
        }

        [MonitoredTest("Should have a gender groupbox"), Order(2)]
        public void _2_ShouldHaveAGenderGroupBox()
        {
            Assert.That(_genderGroupBox, Is.Not.Null, () => "Could not find a GroupBox control with header 'Geslacht'");

            var radioButtons = _genderGroupBox.FindVisualChildren<RadioButton>().ToList();
            Assert.That(radioButtons.Count, Is.EqualTo(2), () => "Could not find 2 RadioButtons within the gender groupbox");
        }

        [MonitoredTest("Should have a age groupbox"), Order(3)]
        public void _3_ShouldHaveAnAgeGroupBox()
        {
            Assert.That(_ageGroupBox, Is.Not.Null, () => "Could not find a GroupBox control with header 'Leeftijd'");

            var radioButtons = _ageGroupBox.FindVisualChildren<RadioButton>().ToList();
            Assert.That(radioButtons.Count, Is.EqualTo(4), () => "Could not find 4 RadioButtons within the age groupbox");
        }

        [MonitoredTest("Should have a button"), Order(4)]
        public void _4_ShouldHaveAButton()
        {
            Assert.That(_button, Is.Not.Null, () => "Could not find a Button control");
            Assert.That(_button.Content, Contains.Substring("bereken").IgnoreCase, () => "The 'Content' of the button should be 'Bereken'");
        }

        [MonitoredTest("Should check which radiobuttons are checked after button click"), Order(5)]
        public void _5_ShouldCheckWhichRadioButtonsAreCheckedAfterButtonClick()
        {
            var sourceCode = Solution.Current.GetFileContent(@"Exercise14\MainWindow.xaml.cs");
            sourceCode = CodeCleaner.StripComments(sourceCode);

            Assert.That(sourceCode, Contains.Substring("(object sender, RoutedEventArgs e)"), () => "No eventhandler method found for the click event of the button");
            Assert.That(sourceCode, Contains.Substring(".IsChecked"), () => "No code found where the state of a radiobutton is checked");
        }
    }
}
