using System.Collections.Generic;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ExposeInventoryExtraData
    {
        public IEnumerable<NonCharacterObject> Contents { get; set; }

        public GameAction[] PossibleUses { get; set; }

        public int Money { get; set; }
    }
}
