using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class BuyActionPerformer:TakeFromActionPerformer
    {
        public override bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (!base.CanPerform(actionTemplate))
                return false;
            if (!actionTemplate.ActiveCharacter.Walet.CanPerformTransaction(actionTemplate.TargetCharacter, ((ITransactionalObject)actionTemplate.TargetObject).BuyPrice))
                return false;
            return true;
        }
        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult
                {
                    ForAction = actionTemplate,
                    ResultType = ActionResultType.NotOk
                };

            actionTemplate.ActiveCharacter.Walet.PerformTransaction(actionTemplate.TargetCharacter,
                                                                    ((ITransactionalObject) actionTemplate.TargetObject)
                                                                        .BuyPrice);
            return PerformWithoutCheck(actionTemplate);
        }

    }
}
