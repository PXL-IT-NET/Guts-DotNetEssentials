using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExerciseBeetle.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "ExerciseBeetle", @"ExerciseBeetle\MainWindow.xaml;ExerciseBeetle\MainWindow.xaml.cs"),
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

        [MonitoredTest("Should have a private member of class Beetle"), Order(1)]
        public void _1_ShouldHaveAPrivateBeetleMember()
        {
            Assert.Fail();
        }
    }
}
