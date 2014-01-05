using System;

namespace GetTheMilk.Utils
{
    public static class Randomizer
    {
        public static int GetRandom(int min, int max)
        {
            Random randomSelector = new Random();
            return randomSelector.Next(min, max);
        }
    }
}
