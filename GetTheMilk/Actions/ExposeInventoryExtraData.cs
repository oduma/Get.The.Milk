using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ExposeInventoryExtraData
    {
        public NonCharacterObject[] Contents { get; set; }

        public GameAction[] PossibleUses { get; set; }

        public int Money { get; set; }
    }
}
