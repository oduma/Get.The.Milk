using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Objects
{
    public abstract class AnyKey:Tool
    {
        protected AnyKey()
        {
            BlockMovement = false;
            BuyPrice = 100;
            SellPrice = 80;
        }

        public  override bool AllowsAction(GameAction a)
        {
            return (a is Open);
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
                return (o is Character && (a is Pick || a is Keep || a is GiveTo || a is Buy || a is Sell));
        }
    }
}
