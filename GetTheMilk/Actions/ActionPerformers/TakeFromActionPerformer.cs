using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class TakeFromActionPerformer:ObjectTransferToActiveCharacterPerformer
    {
        public override bool CanPerform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (!base.CanPerform(actionTemplate))
                return false;
            if (actionTemplate.TargetCharacter != null && (actionTemplate.TargetObject.StorageContainer.Owner.Name != actionTemplate.TargetCharacter.Name))
                return false;
            return true;
        }

        public override Base.PerformActionResult Perform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
            {
                return PerformWithoutCheck(actionTemplate);
            }
            return new PerformActionResult
            {
                ForAction = actionTemplate,
                ResultType = ActionResultType.NotOk
            };

        }

        protected PerformActionResult PerformWithoutCheck(ObjectTransferActionTemplate actionTemplate)
        {
            var addedOk = actionTemplate.ActiveCharacter.Inventory.Add(actionTemplate.TargetObject);
            return new PerformActionResult
            {
                ForAction = actionTemplate,
                ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
            };
        }
    }
}
