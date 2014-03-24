using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class ExposeInventory:GameAction
    {
        public override bool CanPerform()
        {
            return TargetCharacter.AllowsAction(this);
        }

        public ExposeInventory()
        {
            Name = new Verb {Infinitive = "To Expose", Past = "exposed", Present = "expose"};
            ActionType = ActionType.ExposeInventory;
        }

        public GameAction[] AllowedNextActions { get; set; }

        public bool IncludeWallet { get; set; }

        public override ActionResult Perform()
        {
            var result = new ActionResult {ResultType = ActionResultType.Ok, ForAction=this};
            if (!CanPerform())
            {
                result.ResultType = ActionResultType.NotOk;
                return result;
            }
            result.ExtraData = new ExposeInventoryExtraData
                                   {
                                       Contents = TargetCharacter.Inventory,
                                       PossibleUses = AllowedNextActions,
                                       Money = (IncludeWallet)?TargetCharacter.Walet.CurrentCapacity:0
                                   };

            return result;
        }
        public override GameAction CreateNewInstance()
        {
            return new ExposeInventory();
        }

    }
}
