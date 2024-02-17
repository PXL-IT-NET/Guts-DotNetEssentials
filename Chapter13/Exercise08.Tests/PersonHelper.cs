using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise08.Tests
{
    internal class PersonHelper
    {
        public static readonly string PersonTypeName = "Exercise08.Person";
        public static readonly string GenderTypeName = "Exercise08.GenderType";
        public static readonly string PersonAssembly = "Exercise08";
        public static readonly Type PersonType = Type.GetType($"{PersonTypeName}, {PersonAssembly}");

        public const string LastNameProperty = "LastName";
        public const string FirstNameProperty = "FirstName";
        public const string AddressProperty = "Address";
        public const string BirthDateProperty = "BirthDate";
        public const string PhoneProperty = "Phone";
        public const string GenderProperty = "Gender";

        public static object CreatePerson()
        {
            Assert.That(PersonTypeName, Is.Not.Null, $"There should be a class named Person");
            Assert.That(GenderTypeName, Is.Not.Null, $"There should be an enum called Gender");
            object person = null;
            try
            {
                person = Activator.CreateInstance(PersonType);
            }
            catch
            { 
                // swallow exception
            }
            return person;
        }

        public static object GetPropertyValue(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            return property.GetValue(obj);
        }

        public static void SetPropertyValue(object obj, string propertyName, object newValue)
        {
            var property = obj.GetType().GetProperty(propertyName);
            property.SetValue(obj, newValue);
        }
    }
}
