using System;

namespace PixelPass
{
    public class PasswordValidator
    {
        private const int MinimumLength = 5;
        private const int AverageLength = 10;

        private const string alfabetSmallCaps = "abcdefghijklmnopqrstuvwxyz";
        private const string alfabetUpperCaps = "ABCEDFGHIJKLMNOPQRSTUVWXYZ";
        private const string digits = "0123456789";

        public static Strength CalculateStrength(string password)
        {
            return Strength.Weak;
        }
    }

    public enum Strength
    {
        Weak,
        Average,
        Strong
    }
}