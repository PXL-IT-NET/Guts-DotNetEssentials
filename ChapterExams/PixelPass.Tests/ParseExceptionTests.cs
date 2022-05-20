using Guts.Client.Core;
using NUnit.Framework;
using PixelPass.Tests.Helpers;
using System.Threading;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\ParseException.cs")]
[Apartment(ApartmentState.STA)]
public class ParseExceptionTests
{
    [MonitoredTest("ParseException - Inherits Exception"), Order(1)]
    public void _01_Class_ShouldInheritFrom_ParseException()
    {
        Assert.That(ParseExceptionHelper.ClassExistsAndInheritsFromParseException(),
                    "ParseException should exist and should inherit from Exception");
    }
}
