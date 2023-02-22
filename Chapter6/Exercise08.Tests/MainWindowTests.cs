using System.Threading;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using NUnit.Framework;

namespace Exercise08.Tests
{
    [ExerciseTestFixture("dotNet1", "H06", "Exercise08", @"Exercise08\MainWindow.xaml")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _testWindow;

        private bool _hasNameLabel;
        private bool _hasPasswordLabel;
        private bool _hasNameTextBox;
        private bool _hasPasswordBox;
        private bool _hasOkButton;
        private bool _hasCancelButton;

        [OneTimeSetUp]
        public void Setup()
        {
            _testWindow = new MainWindow();
            _hasNameLabel = _testWindow.GetPrivateFieldValueByName<Label>("nameLabel") != null;
            _hasPasswordLabel = _testWindow.GetPrivateFieldValueByName<Label>("passwordLabel") != null;
            _hasNameTextBox = _testWindow.GetPrivateFieldValue<TextBox>() != null;
            _hasPasswordBox = _testWindow.GetPrivateFieldValue<PasswordBox>() != null;
            _hasOkButton = _testWindow.GetPrivateFieldValueByName<Button>("okButton") != null;          
            _hasCancelButton = _testWindow.GetPrivateFieldValueByName<Button>("cancelButton") != null;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testWindow?.Close();
        }

        [MonitoredTest("Should have labels")]
        public void ShouldHaveLabels()
        {
            Assert.That(_hasNameLabel, Is.True, () => "No Label found with text 'Naam:'");
            Assert.That(_hasPasswordLabel, Is.True, () => "No Label found with text 'Paswoord:'");
        }

        [MonitoredTest("Should have name input control")]
        public void ShouldHaveNameInputControl()
        {
            Assert.That(_hasNameTextBox, Is.True, () => "No suitable input control found for the name. Also make sure the name of the control contains 'name'");
        }

        [MonitoredTest("Should have password input control")]
        public void ShouldHavePasswordInputControl()
        {
            Assert.That(_hasPasswordBox, Is.True, () => "No suitable input control found for the password. Also make sure the name of the control contains 'password'");
        }

        [MonitoredTest("Should have buttons")]
        public void ShouldHaveButtons()
        {
            Assert.That(_hasOkButton, Is.True, () => "No private Button field found with a name that contains 'ok'. Create a Button with a name like 'okButton'");
            Assert.That(_hasCancelButton, Is.True, () => "No private Button field found with a name that contains 'cancel'. Create a Button with a name like 'cancelButton'");
        }
    }
}
