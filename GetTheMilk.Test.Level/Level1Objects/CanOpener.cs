using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.TestLevel.Level1Objects
{
    public class CanOpener:Tool
    {
        public CanOpener()
        {
            BlockMovement = false;
            BuyPrice = 5;
            SellPrice = 1;
        }
        public override string Name
        {
            get { return "Can Opener"; }
        }
    }
}
