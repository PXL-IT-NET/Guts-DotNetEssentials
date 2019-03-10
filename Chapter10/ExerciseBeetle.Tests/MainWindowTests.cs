using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeetleGame.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "BeetleGame", @"BeetleGame\MainWindow.xaml;BeetleGame\MainWindow.xaml.cs"),
     Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        // Todo
        // Test Window functionality

        [SetUp]
        public void Setup()
        { }

        [TearDown]
        public void TearDown()
        { }

        [MonitoredTest("MainWindow - Should have a private member of class Beetle"), Order(1)]
        public void _99_ShouldHaveAPrivateBeetleMember()
        {
            Assert.Fail();
        }
    }
}
