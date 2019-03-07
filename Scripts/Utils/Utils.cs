using System;
using System.Collections.Generic;
using System.Globalization;

namespace AssemblyCSharp
{
    public static class ListRandomizer
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class TimeStamp
    {
        public static string Get()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff",
                                                CultureInfo.InvariantCulture);
        }
    }
}