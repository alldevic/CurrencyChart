using System;

namespace CurrencyChart.Server.Services
{
    public static class RandomNumberGenerator
    {
        static readonly Random Rnd1 = new Random();

        public static int RandomScalingFactor() => Rnd1.Next(10);
    }
}