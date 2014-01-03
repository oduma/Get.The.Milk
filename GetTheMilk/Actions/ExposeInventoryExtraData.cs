using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ExposeInventoryExtraData
    {
        public IPositionableObject[] Contents { get; set; }

        public GameAction[] PossibleUses { get; set; }

        public int Money { get; set; }
    }
}
