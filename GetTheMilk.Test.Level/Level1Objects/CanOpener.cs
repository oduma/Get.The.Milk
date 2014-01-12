using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class CanOpener:Tool
    {
        public CanOpener()
        {
            BlockMovement = false;
            BuyPrice = 5;
            SellPrice = 1;
            Name = new Noun { Main = "Can Opener" };
        }
    }
}
