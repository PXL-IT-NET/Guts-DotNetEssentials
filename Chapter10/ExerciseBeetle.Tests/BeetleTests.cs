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
    [ExerciseTestFixture("dotNet1", "H10", "ExerciseBeetle", @"ExerciseBeetle\Beetle.cs"),
     Apartment(ApartmentState.STA)]
    public class BeetleTests
    {
        private object _beetleObject = null;
        
        // Todo
        // - test if Beetle class exists
        // - test all members of Beetle

        [SetUp]
        public void Setup()
        { }

        [TearDown]
        public void TearDown()
        { }

        [MonitoredTest("There should be a class named Beetle"), Order(1)]
        public void _1_ShouldHaveAClassNamedBeetle()
        {
            Assert.Fail();
        }
    }
}
