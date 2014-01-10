using GetTheMilk.Utils;

namespace GetTheMilk.Settings
{
    public static class GameSettings
    {
        public static bool AllowChoiceOfDefensiveWeapons
        {
            get { return false; }
        }

        public static bool FightersRightHanded
        {
            get { return true; }
        }

        public static string InteractiveMode { get { return "TextBased"; } }

        public static int MaximumExperience { get { return 1000; } }

        public static int FullDefaultHealth
        {
            get { return 10; }
        }

        public static int DefaultRunDistance
        {
            get { return 3; }
        }

        public static int DefaultWalkDistance   
        {
            get { return 1; }
        }

        public static int DefaultWalletMaxCapacity
        {
            get { return 200; }
        }

        public static int MinimumStartingExperience
        {
            get { return 1; }
        }

        public static int MinimumStartingMoney
        {
            get { return 20; }
        }

        public static int MaximumAvailableBonusPoints
        {
            get { return 100; }
        }

        public static int GetRandomMoneyBoost()
        {
            return Randomizer.GetRandom(5, 20);
        }

        public static int GetRandomExperienceBoost()
        {
            return Randomizer.GetRandom(10, 50);
        }
    }
}
