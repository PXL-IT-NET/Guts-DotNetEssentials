using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PixelPass.Tests.Helpers;

internal class AccountInfoCollectionHelper : BaseHelper
{
    static readonly string TypeName = "PixelPass.AccountInfoCollection";
    static readonly Type AccountInfoCollectionType = Type.GetType($"{TypeName}, {AssemblyName}");

    public static bool ImplementsIAccountInfoCollection()
    {
        Assert.That(AccountInfoCollectionType, Is.Not.Null, $"There should be a class named {TypeName}, did you remove it accidentally?");
        return AccountInfoCollectionType.IsAssignableTo(typeof(IAccountInfoCollection));
    }

    public static List<AccountInfo> CreateTestListWithAccountInfo()
    {
        return new List<AccountInfo>()
        {
            new AccountInfo
            {
                Title = "Kinepolis",
                Username = "piet.pienter@student.pxl.be",
                Password = "6Dj8giElqxBm",
                Notes = "www.kinepolis.be",
                Expiration = DateTime.Now - TimeSpan.FromDays(10), // Expired
            },
            new AccountInfo
            {
                Title = "Eventbrite",
                Username = "piet.pienter@student.pxl.be",
                Password = "S8vX1ZbCwxeV",
                Notes = "www.eventbrite.be",
                Expiration = DateTime.Now + TimeSpan.FromDays(10), // Not Expired
            }
        };
    }
}