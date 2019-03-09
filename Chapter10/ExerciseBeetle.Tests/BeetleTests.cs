using Guts.Client.Classic;
using Guts.Client.Shared;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        [MonitoredTest("Beetle - Beetle class should have all required properties")]
        public void _2_ShouldHaveAllProperties()
        {
            var properties = _beetleObject.GetType().GetProperties();
            string[] expectedPropertyNames = { "Speed", "X", "Y", "Size", "Right", "Up", "IsVisible" };
            Type[] expectedPropertyTypes = {typeof(double), typeof(int), typeof(int), typeof(int),
                                            typeof(bool), typeof(bool), typeof(bool)};
            for (int i = 0; i < expectedPropertyNames.Length; i++)
            {
                AssertProperty(properties, expectedPropertyNames[i], expectedPropertyTypes[i],
                           $"{_beetleType} should have a property named ${expectedPropertyNames[i]} of type ${expectedPropertyTypes[i]}.");
            }

        }

        private void AssertProperty(PropertyInfo[] properties, string expectedPropertyName,
                                    Type expectedPropertyType, string message)
        {
            var property = properties.FirstOrDefault(p => p.Name == expectedPropertyName
                                                     && p.PropertyType == expectedPropertyType);
            Assert.That(property, Is.Not.Null, () => message);
        }
    }
}
