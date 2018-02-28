using System;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Guts.Client;
using Guts.Client.TestTools;
using Guts.Client.TestTools.WPF;
using NUnit.Framework;

namespace Exercise10.Tests
{
    [MonitoredTestFixture("dotNet1", 6, 10), Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;

        private bool _hasNameLabel;
        private bool _hasPasswordLabel;
        private bool _hasNameTextBox;
        private bool _hasPasswordBox;
        private Button _okButton;
        private bool _hasCancelButton;
        private ProgressBar _progressBar;
        private DispatcherTimer _dispatcherTimer;
        private EventHandler _tickEventHandler;

        [OneTimeSetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();
            _hasNameLabel = _testWindow.GetContentControlByPartialContentText<Label>("naam") != null;
            _hasPasswordLabel = _testWindow.GetContentControlByPartialContentText<Label>("paswoord") != null;
            _hasNameTextBox = _testWindow.GetPrivateField<TextBox>() != null;
            _hasPasswordBox = _testWindow.GetPrivateField<PasswordBox>() != null;
            _okButton = _testWindow.GetContentControlByPartialContentText<Button>("ok");
            _hasCancelButton = _testWindow.GetContentControlByPartialContentText<Button>("cancel") != null;
            _progressBar = _testWindow.GetUIElements<ProgressBar>().FirstOrDefault();

            _dispatcherTimer = _testWindow.GetPrivateField<DispatcherTimer>();
            if (_dispatcherTimer != null)
            {
                _tickEventHandler = _dispatcherTimer.GetPrivateFieldValueByName<EventHandler>(nameof(DispatcherTimer.Tick));
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testWindow.Dispose();
        }

        [MonitoredTest("Should have labels"), Order(1)]
        public void _1_ShouldHaveLabels()
        {
            Assert.That(_hasNameLabel, Is.True, () => "No Label found with text 'Naam:'");
            Assert.That(_hasPasswordLabel, Is.True, () => "No Label found with text 'Paswoord:'");
        }

        [MonitoredTest("Should have name input control"), Order(2)]
        public void _2_ShouldHaveNameInputControl()
        {
            Assert.That(_hasNameTextBox, Is.True, () => "No suitable input control found for the name. Also make sure the name of the control contains 'name'");
        }

        [MonitoredTest("Should have password input control"), Order(3)]
        public void _3_ShouldHavePasswordInputControl()
        {
            Assert.That(_hasPasswordBox, Is.True, () => "No suitable input control found for the password. Also make sure the name of the control contains 'password'");
        }

        [MonitoredTest("Should have buttons"), Order(4)]
        public void _4_ShouldHaveButtons()
        {
            Assert.That(_okButton, Is.Not.Null, () => "No private Button field found with a name that contains 'ok'. Create a Button with a name like 'okButton'");
            Assert.That(_hasCancelButton, Is.True, () => "No private Button field found with a name that contains 'cancel'. Create a Button with a name like 'cancelButton'");
        }

        [MonitoredTest("Should have a progressbar"), Order(5)]
        public void _5_ShouldHaveProgressBar()
        {
            AssertHasProgressBar();
        }

        [MonitoredTest("Should have a DispatcherTimer and a method that handles its ticks"), Order(6)]
        public void _6_ShouldHaveADispatcherTimerAndMethodThatHandlesTicks()
        {
            AssertHasDispatcherTimer();

            Assert.That(_tickEventHandler, Is.Not.Null, () => "No event handler set for the Tick event of the DispatcherTimer");
            var invocationList = _tickEventHandler.GetInvocationList();
            Assert.That(invocationList.Length, Is.GreaterThan(0), () => "No event handler set for the Tick event of the DispatcherTimer");
        }

        [MonitoredTest("Should tick at least once per second"), Order(7)]
        public void _7_ShouldTickAtLeastOncePerSecond()
        {
            AssertHasDispatcherTimer();

            Assert.That(_dispatcherTimer.Interval, Is.Not.Null, () => "No interval set for the Tick event of the DispatcherTimer");
            Assert.That(_dispatcherTimer.Interval, Is.Not.GreaterThan(TimeSpan.FromSeconds(1)), () => "The interval set for the Tick event of the DispatcherTimer should be one second or less.");
            Assert.That(_dispatcherTimer.IsEnabled, Is.True, () => "The dispatcher timer is not started. Use the 'Start' method to start the timer");
        }

        [MonitoredTest("Should show correct amount of progress on every tick"), Order(8)]
        public void _8_ShouldShowCorrectAmountOfProgressOnEveryTick()
        {
            AssertHasDispatcherTimer();
            AssertHasProgressBar();

            _dispatcherTimer.Stop();

            _progressBar.Value = 0;

            var expectedProgressPerTick = GetExpectedProgressPerTick();

            var numberOfTicks = 3;
            InvokeTickEvent(numberOfTicks);

            var expectedProgress = numberOfTicks * expectedProgressPerTick;
            Assert.That(_progressBar.Value, Is.EqualTo(expectedProgress),
                () =>
                    $"After {numberOfTicks} ticks the value of the progressbar is expected to be {expectedProgress} when the progressbar maximum is set to {_progressBar.Maximum}.");
        }

        [MonitoredTest("Should disable button and show message after 5 seconds"), Order(9)]
        public void _9_ShouldDisableButtonAndShowMessageAfter5Seconds()
        {
            AssertHasDispatcherTimer();
            AssertHasProgressBar();

            _dispatcherTimer.Stop();

            _progressBar.Value = 0;

            var expectedProgressPerTick = GetExpectedProgressPerTick();

            var numberOfTicks = (int)(_progressBar.Maximum / expectedProgressPerTick) + 1;

            _dispatcherTimer.IsEnabled = true;
            InvokeTickEvent(numberOfTicks);

            Assert.That(_progressBar.Value, Is.GreaterThanOrEqualTo(_progressBar.Maximum), () => $"After {numberOfTicks} ticks the progressbar should be completely filled.");
            Assert.That(_dispatcherTimer.IsEnabled, Is.False, () => "When the progressbar is filled the dispatcher timer should be stopped.");
            Assert.That(_okButton.IsEnabled, Is.False, () => "When the progressbar is filled the OK button should be disabled");
        }

        private double GetExpectedProgressPerTick()
        {
            var intervalInMilliseconds = _dispatcherTimer.Interval.TotalMilliseconds;
            return (_progressBar.Maximum / 5.0) * (intervalInMilliseconds / 1000);
        }

        private void AssertHasProgressBar()
        {
            Assert.That(_progressBar, Is.Not.Null, () => "No ProgressBar component found.");
        }

        private void AssertHasDispatcherTimer()
        {
            Assert.That(_dispatcherTimer, Is.Not.Null, () => "No private field found of the type DispatcherTimer");

        }

        private void InvokeTickEvent(int numberOfTimes)
        {
            for (int i = 0; i < numberOfTimes; i++)
            {
                InvokeTickEvent();
            }
        }

        private void InvokeTickEvent()
        {
            if (_tickEventHandler == null) return;

            var invocationList = _tickEventHandler.GetInvocationList();
            Assert.That(invocationList.Length, Is.GreaterThan(0));

            foreach (var handlerDelegate in invocationList)
            {
                handlerDelegate.Method.Invoke(handlerDelegate.Target, new Object[] { _dispatcherTimer, EventArgs.Empty });
            }
        }
    }
}
