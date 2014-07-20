using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class SellActionPerformer:GiveToActionPerformer
    {
        public override bool CanPerform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (!base.CanPerform(actionTemplate))
                return false;
            if (!actionTemplate.TargetCharacter.Walet.CanPerformTransaction(actionTemplate.ActiveCharacter, ((ITransactionalObject)actionTemplate.TargetObject).SellPrice))
                return false;
            return true;
        }

        public override Base.PerformActionResult Perform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult
                {
                    ForAction = actionTemplate,
                    ResultType = ActionResultType.NotOk
                };

            actionTemplate.TargetCharacter.Walet.PerformTransaction(actionTemplate.ActiveCharacter,
                                                                    ((ITransactionalObject) actionTemplate.TargetObject)
                                                                        .SellPrice);
            return PerformWithoutCheck(actionTemplate);
        }
    }
}
