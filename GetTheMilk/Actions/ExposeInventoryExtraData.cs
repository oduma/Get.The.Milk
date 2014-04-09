using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions
{
    public class ExposeInventoryExtraData
    {
        public ObjectWithPossibleActions[] Contents { get; set; }

        public GameAction FinishingAction { get; set; }

        public int Money { get; set; }
    }
}
