using Guts.Client.Classic;
using Guts.Client.Classic.TestTools.WPF;
using Guts.Client.Shared;
using Guts.Client.Shared.TestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BeetleGame.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\MainWindow.xaml;BeetleGame\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private TestWindow<MainWindow> _testWindow;
        private Beetle _beetleObject;
        private DispatcherTimer _timerObject;
        private Slider _speedSlider;
        private Slider _sizeSlider;
        private Button _startButton, _resetButton;
        private Button _leftButton, _downButton, _upButton, _rightButton;


        [SetUp]
        public void Setup()
        {
            _testWindow = new TestWindow<MainWindow>();
            _beetleObject = _testWindow.GetPrivateField<Beetle>();
            _timerObject = _testWindow.GetPrivateField<DispatcherTimer>();
            _speedSlider = _testWindow.GetUIElements<Slider>().FirstOrDefault(
                (slider) => slider.Name.ToUpper().Contains("SPEED"));
            _sizeSlider = _testWindow.GetUIElements<Slider>().FirstOrDefault(
                (slider) => slider.Name.ToUpper().Contains("SIZE"));
            _startButton = _testWindow.GetContentControlByPartialContentText<Button>("Start");
            _resetButton = _testWindow.GetContentControlByPartialContentText<Button>("Reset");
            _leftButton = _testWindow.GetContentControlByPartialContentText<Button>("Left");
            _rightButton = _testWindow.GetContentControlByPartialContentText<Button>("Right");
            _downButton = _testWindow.GetContentControlByPartialContentText<Button>("Down");
            _upButton = _testWindow.GetContentControlByPartialContentText<Button>("Up");
        }

        [TearDown]
        public void TearDown()
        {
            _testWindow?.Dispose();
        }

        [MonitoredTest("MainWindow - Should have a private member of class Beetle"), Order(1)]
        public void _M01_ShouldHaveAPrivateBeetleMember()
        {
            Assert.That(_beetleObject, Is.Not.Null, "Mainwindow should have a private member variable of class Beetle");
        }

        [MonitoredTest("MainWindow - Should have a private member of class DispatcherTimer"), Order(2)]
        public void _M02_ShouldHaveAPrivateDispatcherTimerMember()
        {
            Assert.That(_timerObject, Is.Not.Null, "Mainwindow should have a private member variable of class DispatcherTimer");
        }

        [MonitoredTest("MainWindow - Should have a slider for controlling Speed"), Order(3)]
        public void _M03_ShouldHaveSliderForSpeed()
        {
            double expectedMinimum = 0.5;
            double expectedMaximum = 10;
            Assert.That(_speedSlider, Is.Not.Null, "Mainwindow should have a Slider for speed (speedSlider)");
            Assert.That(_speedSlider.Minimum, Is.EqualTo(expectedMinimum),
                $"Slider for controlling speed should have a minimum value of {expectedMinimum}");
            Assert.That(_speedSlider.Maximum, Is.EqualTo(expectedMaximum), 
                $"Slider for controlling speed should have a maximum value of {expectedMaximum}");
        }

        [MonitoredTest("MainWindow - Should have a slider for controlling Size"), Order(4)]
        public void _M04_ShouldHaveSliderForSize()
        {
            double expectedMinimum = 10;
            double expectedMaximum = 20;
            double expectedTickFrequency = 2.0;
            bool expectedIsSnapToTickEnabled = true;
            Assert.That(_sizeSlider, Is.Not.Null, "Mainwindow should have a Slider for size (sizeSlider)");
            Assert.That(_sizeSlider.Minimum, Is.EqualTo(expectedMinimum),
                $"Slider for controlling size should have a minimum value of {expectedMinimum}");
            Assert.That(_sizeSlider.Maximum, Is.EqualTo(expectedMaximum), 
                $"Slider for controlling size should have a maximum value of {expectedMaximum}");
            Assert.That(_sizeSlider.TickFrequency, Is.EqualTo(expectedTickFrequency),
                $"Slider for controlling size should have a tick frequency of {expectedTickFrequency}");
            Assert.That(_sizeSlider.IsSnapToTickEnabled, Is.EqualTo(expectedIsSnapToTickEnabled),
                $"Slider for controlling size should have a IsSnapToTickEnabled property set to {expectedIsSnapToTickEnabled}");
        }

        [MonitoredTest("MainWindow - Should have buttons for controlling start and reset"), Order(5)]
        public void _M05_ShouldHaveButtonsForStartReset()
        {
            Assert.That(_startButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Start>");
            Assert.That(_resetButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Reset>");
        }

        [MonitoredTest("MainWindow - Should have buttons for controlling left, right, up, down control"), Order(6)]
        public void _M06_ShouldHaveButtonsForLeftRightUpDown()
        {
            Assert.That(_leftButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Left>");
            Assert.That(_rightButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Right>");
            Assert.That(_upButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Up>");
            Assert.That(_downButton, Is.Not.Null, @"MainWindow should contain a Button labeled <Down>");
        }

        [MonitoredTest("MainWindow - Hitting Start Button should change its content text to Stop (and back)"), Order(7)]
        public void _M07_ShouldChangeContentLabelToStopWhenHittingStartButtonAndBack()
        {
            Assert.That(_startButton.Content, Is.EqualTo("Start"), @"MainWindow should contain a Button labeled <Start>");
            _startButton.FireClickEvent();
            Assert.That(_startButton.Content, Is.EqualTo("Stop"), @"After clicking the Start-button, the content text should change to <Stop>");
            _startButton.FireClickEvent();
            Assert.That(_startButton.Content, Is.EqualTo("Start"), @"After clicking the Stop-button, the content text should change to <Start>");
        }


    }
}
