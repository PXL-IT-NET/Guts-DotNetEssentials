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
            Assert.That(_speedSlider, Is.Not.Null, "Mainwindow should have a Slider for speed (speedSlider)");
        }

        [MonitoredTest("MainWindow - Should have a slider for controlling Size"), Order(4)]
        public void _M04_ShouldHaveSliderForSize()
        {
            Assert.That(_sizeSlider, Is.Not.Null, "Mainwindow should have a Slider for size (sizeSlider)");
        }


    }
}
