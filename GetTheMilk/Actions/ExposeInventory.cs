using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;

namespace GetTheMilk.Actions
{
    public class ExposeInventory:GameAction
    {
        public override bool CanPerform()
        {
            return ActiveCharacter.AllowsAction(this) && TargetCharacter.AllowsIndirectAction(this,ActiveCharacter);
        }

        public ExposeInventory()
        {
            Name = new Verb {Infinitive = "To Expose", Past = "exposed", Present = "expose"};
            ActionType = ActionType.ExposeInventory;
            StartingAction = false;

        }

        public InventorySubActionType[] AllowedNextActionTypes { get; set; }

        public bool IncludeWallet { get; set; }
        public bool SelfInventory { get; set; }

        public override ActionResult Perform()
        {
            var result = new ActionResult {ResultType = ActionResultType.Ok, ForAction=this};
            var tempTargetCharacter = TargetCharacter;
            if (SelfInventory)
                TargetCharacter = ActiveCharacter;
            if (!CanPerform())
            {
                result.ResultType = ActionResultType.NotOk;
                return result;
            }
            var actionsFactory = ActionsFactory.GetFactory();
            result.ExtraData = new ExposeInventoryExtraData
                                   {
                                       Contents = ActiveCharacter.Inventory,
                                       PossibleUses = (AllowedNextActionTypes==null)?null:AllowedNextActionTypes.Select(a =>
                                                                  {
                                                                      var act = actionsFactory.CreateNewActionInstance(a.ActionType);
                                                                      act.ActiveCharacter = TargetCharacter;
                                                                      act.TargetCharacter = (act is TwoCharactersAction) ? tempTargetCharacter : ActiveCharacter;
                                                                      act.FinishTheInteractionOnExecution =
                                                                          a.FinishInventoryExposure;
                                                                      return act;
                                                                  }).ToArray(),
                                       Money = (IncludeWallet)?ActiveCharacter.Walet.CurrentCapacity:0
                                   };

            return result;
        }
        public override GameAction CreateNewInstance()
        {
            return new ExposeInventory();
        }

    }
}
