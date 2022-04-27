using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPass.Tests.Helpers
{
    internal class CreateAccountInfoWindowHelper : BaseHelper
    {
        static readonly string TypeName = "PixelPass.CreateAccountInfoWindow";
        static readonly Type CreateAccountInfoWindowType = Type.GetType($"{TypeName}, {AssemblyName}");
        static readonly string IAccountInfoCollectionName = "PixelPass.IAccountInfoCollection";
        static readonly Type IAccountInfoCollectionType = Type.GetType($"{IAccountInfoCollectionName}, {AssemblyName}");

        public static bool HasConstructorWithParameterIAccountInfoCollection()
        {
            Assert.That(CreateAccountInfoWindowType, Is.Not.Null,
                $"There should be a class named {TypeName}, did you remove it accidentally?");
            Assert.That(IAccountInfoCollectionType, Is.Not.Null,
                $"There should be an interface named {IAccountInfoCollectionName}, did you remove it accidentally?");
            return CreateAccountInfoWindowType.GetConstructor(new[] { IAccountInfoCollectionType }) is not null;
        }

        public static CreateAccountInfoWindow Create(IAccountInfoCollection accountInfoCollection)
        {
            CreateAccountInfoWindow window = null;
            object[] parameters = { accountInfoCollection };

            if (HasConstructorWithParameterIAccountInfoCollection())
            {
                window = Activator.CreateInstance(CreateAccountInfoWindowType, parameters) as CreateAccountInfoWindow;
            }

            return window;
        }
    }
}
