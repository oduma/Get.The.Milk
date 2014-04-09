using System.Collections.Generic;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ObjectWithPossibleActions
    {
        public NonCharacterObject Object { get; set; }

        public GameAction[] PossibleUsses { get; set; }
    }
}
