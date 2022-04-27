using Guts.Client.Core;
using NUnit.Framework;
using PixelPass.Tests.Helpers;
using System.Threading;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\AccountInfoCollection.cs")]
[Apartment(ApartmentState.STA)]
public class AccountInfoCollectionTests
{

    [MonitoredTest("AccountInfoCollection - Implement IAccountInfoCollection"), Order(1)]
    public void _01_Class_ShouldImplement_IAccountInfoCollection()
    {
        Assert.That(AccountInfoCollectionHelper.ImplementsIAccountInfoCollection(),
                    "AccountInfoCollection should implement interface IAccountInfoCollection");
    }
}
