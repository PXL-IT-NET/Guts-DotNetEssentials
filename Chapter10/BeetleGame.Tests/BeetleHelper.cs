using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BeetleGame.Tests
{
    public static class BeetleHelper
    {
        public static readonly string BeetleTypeName = "BeetleGame.Beetle";
        public static readonly string BeetleAssembly = "BeetleGame";
        public static readonly Type BeetleType = Type.GetType($"{BeetleTypeName}, {BeetleAssembly}");

        public const string SpeedProperty = "Speed";
        public const string XProperty = "X";
        public const string YProperty = "Y";
        public const string SizeProperty = "Size";
        public const string RightProperty = "Right";
        public const string UpProperty = "Up";
        public const string VisibleProperty = "IsVisible";
        
        public static object CreateBeetle(Canvas canvas, int x, int y, int size)
        {
            Assert.That(BeetleType, Is.Not.Null, $"There should be a class named {BeetleTypeName}, did you remove it accidentally?");
            object[] parameters = new object[] { canvas, x, y, size };
            object beetle = null;
            try
            {
                beetle = Activator.CreateInstance(BeetleType, parameters);
            }
            catch (Exception)
            { }; // swallow exception if beetle can't be instantiated

            return beetle;
        }

        public static object GetRightProperty(object beetleObject)
            => GetPropertyValue(beetleObject, RightProperty);

        public static void SetRightProperty(object beetleObject, bool newValue)
            => SetPropertyValue(beetleObject, RightProperty, newValue);

        public static object GetSpeedProperty(object beetleObject)
            => GetPropertyValue(beetleObject, SpeedProperty);

        public static void SetSpeedPropertyValue(object beetleObject, double newValue)
            => SetPropertyValue(beetleObject, SpeedProperty, newValue);

        public static object GetUpProperty(object beetleObject)
            => GetPropertyValue(beetleObject, UpProperty);

        public static void SetUpProperty(object beetleObject, bool newValue)
            => SetPropertyValue(beetleObject, UpProperty, newValue);

        public static void SetYProperty(object beetleObject, int newValue)
            => SetPropertyValue(beetleObject, YProperty, newValue);

        public static void SetXProperty(object beetleObject, int newValue)
            => SetPropertyValue(beetleObject, XProperty, newValue);

        public static object GetSizeProperty(object beetleObject)
            => GetPropertyValue(beetleObject, SizeProperty);

        public static object GetIsVisibleProperty(object beetleObject)
            => GetPropertyValue(beetleObject, VisibleProperty);

        public static void SetIsVisibleProperty(object beetleObject, bool newValue)
            => SetPropertyValue(beetleObject, VisibleProperty, newValue);

        public static void SetPropertyValue(object beetleObject, string propertyName, object newValue)
        {
            var property = beetleObject.GetType().GetProperty(propertyName);
            property.SetValue(beetleObject, newValue);
        }

        public static object GetPropertyValue(object beetleObject, string propertyName)
        {
            var property = beetleObject.GetType().GetProperty(propertyName);
            return property.GetValue(beetleObject);
        }
    }
}
