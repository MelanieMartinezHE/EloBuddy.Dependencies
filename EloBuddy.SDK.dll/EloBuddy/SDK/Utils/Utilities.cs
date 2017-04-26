namespace EloBuddy.SDK.Utils
{
    using System;

    public static class Utilities
    {
        private static readonly Random random = new Random(Environment.TickCount);

        public static int GetRandomNumber(int min, int max) => 
            random.Next(min, max);
    }
}

