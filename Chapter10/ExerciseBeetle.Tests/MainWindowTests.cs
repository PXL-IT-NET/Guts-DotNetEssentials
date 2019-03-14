using Guts.Client.Classic;
using Guts.Client.Shared;
using Guts.Client.Shared.TestTools;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BeetleGame.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\MainWindow.xaml;BeetleGame\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window;
        private Beetle _beetleObject;
        private DispatcherTimer _timerObject;

        [SetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _beetleObject = ObjectExtensions.GetPrivateFieldValue<Beetle>(_window);
            _timerObject = ObjectExtensions.GetPrivateFieldValue<DispatcherTimer>(_window);
        }

        [TearDown]
        public void TearDown()
        {
            _window?.Close();
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


    }
}
