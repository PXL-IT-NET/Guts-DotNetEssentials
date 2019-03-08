using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExerciseBeetle.Tests
{
    [ExerciseTestFixture("dotNet1", "H10", "ExerciseBeetle", @"ExerciseBeetle\Beetle.cs"),
     Apartment(ApartmentState.STA)]
    public class BeetleTests
    {
        private object _beetleObject = null;
        private readonly string _beetleType = "ExerciseBeetle.Beetle";
        private readonly string _beetleAssembly = "ExerciseBeetle";
        private readonly int _beetleSize = 10;
        private readonly int _beetleX = 40;
        private readonly int _beetleY = 35;

        private Canvas _testCanvas;
        // Todo
        // - test all members of Beetle

        [SetUp]
        public void Setup()
        {
            _testCanvas = new Canvas
            {
                Width = 536, // mimic the same size as in the starter file
                Height = 356
            };
            
            object[] parameters = new object[] { _testCanvas, _beetleSize, _beetleX, _beetleY };
            Type type = Type.GetType($"{_beetleType}, {_beetleAssembly}");
            _beetleObject = Activator.CreateInstance(type, parameters);
        }

        [TearDown]
        public void TearDown()
        { }

        [MonitoredTest("Beetle - There should be a class named Beetle"), Order(1)]
        public void _1_ShouldHaveAClassNamedBeetle()
        {
            Assert.That(_beetleObject, Is.Not.Null, $"Could not create an instance of class {_beetleType}");
        }
    }
}
