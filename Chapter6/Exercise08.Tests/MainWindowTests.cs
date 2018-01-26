using System.Threading;
using System.Windows.Controls;
using Guts.Client;
using Guts.Client.TestTools;
using NUnit.Framework;

namespace Exercise08.Tests
{
    [MonitoredTestFixture("dotNet1", 6, 8), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;

        private bool _hasNameLabel;
        private bool _hasPasswordLabel;
        private bool _hasNameTextBox;
        private bool _hasPasswordBox;
        private bool _hasOkButton;
        private bool _hasCancelButton;

        [OneTimeSetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();
            _hasNameLabel = _testWindow.GetContentControlByPartialContentText<Label>("naam") != null;
            _hasPasswordLabel = _testWindow.GetContentControlByPartialContentText<Label>("paswoord") != null;
            _hasNameTextBox = _testWindow.GetPrivateField<TextBox>() != null;
            _hasPasswordBox = _testWindow.GetPrivateField<PasswordBox>() != null;
            _hasOkButton = _testWindow.GetContentControlByPartialContentText<Button>("ok") != null;          
            _hasCancelButton = _testWindow.GetContentControlByPartialContentText<Button>("cancel") != null;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testWindow.Dispose();
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
