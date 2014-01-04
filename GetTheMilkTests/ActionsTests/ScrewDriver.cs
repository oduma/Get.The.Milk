using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class ScrewDriver:Tool
    {
        public ScrewDriver()
        {
            BlockMovement = false;
            BuyPrice = 5;
            SellPrice = 1;
        }
        public override string Name
        {
            get { return "SkrewDriver"; }
        }

        public override bool AllowsAction(GameAction a)
        {
                return (a is Pick || a is Keep);
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return ((o is ICharacter) && (a is GiveTo || a is Buy || a is Sell || a is Pick || a is Keep));
        }
    }
}
