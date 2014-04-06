using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class CloseInventory:GameAction
    {
        public override bool CanPerform()
        {
            return true;
        }

        public CloseInventory()
        {
            Name = new Verb {Infinitive = "To Close Inventory", Past = "inventory closed", Present = "close inventory"};
            ActionType = ActionType.CloseInventory;
            StartingAction = false;

        }
        public override ActionResult Perform()
        {
            return new ActionResult {ResultType = ActionResultType.Ok, ForAction=this};
        }
        public override GameAction CreateNewInstance()
        {
            return new CloseInventory();
        }
    }
}
