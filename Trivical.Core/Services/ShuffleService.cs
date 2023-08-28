using System;
using System.Collections.Generic;

namespace Trivical.Core.Services
{
    public static class ShuffleService
    {
        private static Random _random = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}