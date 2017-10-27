using System;
using System.Security.Cryptography;

namespace Classes
{
    static class RandomE
    {
        private static Random random = new Random();
        private static RNGCryptoServiceProvider random2 = new RNGCryptoServiceProvider();

        public static int Next()
        {
            return random.Next();
        }

        public static int Next(int MaxValue)
        {
            return random.Next(MaxValue);
        }

        public static int Next(int MinValue, int MaxValue)
        {
            return random.Next(MinValue, MaxValue);
        }

        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
