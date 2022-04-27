using Guts.Client.Core;
using NUnit.Framework;
using System.Threading;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\PasswordValidator.cs")]
[Apartment(ApartmentState.STA)]
public class PasswordValidatorTests
{
    [MonitoredTest("PasswordValidation - Calculate strength with an empty or null string should return weak"), Order(1)]
    public void _01_EmptyOrNull_ShouldReturn_Weak()
    {
        string emptyPassword = string.Empty;
        string nullPassword = null;

        // act + assert
        Assert.That(PasswordValidator.CalculateStrength(emptyPassword), Is.EqualTo(Strength.Weak),
            "An empty string password should be weak");
        Assert.That(PasswordValidator.CalculateStrength(nullPassword), Is.EqualTo(Strength.Weak),
            "A null password should be weak");
    }

    [MonitoredTest("PasswordValidation - Calculate strength with a password that contains less than 5 chars should return weak"), Order(2)]
    public void _02_PasswordLengthLessThanFiveChars_ShouldReturn_Weak()
    {
        // arrange
        string password = "foo"; // short: < 5 chars

        // act
        var strength = PasswordValidator.CalculateStrength(password);

        // assert
        Assert.That(strength, Is.EqualTo(Strength.Weak),
            "A password length less than five chars should be weak");
    }

    [MonitoredTest("PasswordValidation - Calculate strength with password longer than 5 chars but only one kind of char should be weak"), Order(3)]
    [TestCase("abcdefgh", true)] // weak
    [TestCase("ABCDEFGH", true)]
    [TestCase("12345657", true)]
    [TestCase("weakerweaker", true)]
    [TestCase("abcABC", false)] // not weak
    [TestCase("abc123", false)]
    [TestCase("123ABC", false)]

    public void _03_PasswordLongerThanMinimumButOnlyOneKindOfCharsPassword_ShouldReturn_Weak(string password, bool isWeak)
    {
        Assert.That(PasswordValidator.CalculateStrength(password) == Strength.Weak, Is.EqualTo(isWeak),
            "A password longer than five chars but only one kind of char should be weak.");
    }

    [MonitoredTest("PasswordValidation - Calculate strength with password between 5 and 10 chars with only 2 kind of chars should be average"), Order(4)]
    [TestCase("abcABC")] // average
    [TestCase("abc123")]
    [TestCase("123ABC")]
    public void _04_PasswordsBetween5And10WithOnly2KindsOfChars_ShouldReturn_Average(string password)
    {
        Assert.That(PasswordValidator.CalculateStrength(password), Is.EqualTo(Strength.Average));
    }

    [MonitoredTest("PasswordValidation - Calculate strength with password longer than 10 chars with 3 kind of chars should be strong"), Order(5)]
    [TestCase("1b2q3C4khfg")]
    [TestCase("SdEZA34FGGhHJAAA45")]
    public void _05_PasswordsLongerThan10And3KindsOfChars_ShouldReturn_Strong(string password)
    {
        Assert.That(PasswordValidator.CalculateStrength(password), Is.EqualTo(Strength.Strong));
    }

    [MonitoredTest("PasswordValidation - Calculate strength with password longer than 11 chars with only 2 kind of chars should be average"), Order(6)]
    [TestCase("average1234")]
    [TestCase("AVERAGE1234")]
    [TestCase("aVeRaGeAvEr")]
    public void _06_ElevenCharsWithOnly2KindsOfChars_ShouldReturn_Average(string password)
    {
        Assert.That(PasswordValidator.CalculateStrength(password), Is.EqualTo(Strength.Average));
    }
}
