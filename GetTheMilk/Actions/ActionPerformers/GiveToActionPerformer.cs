using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class GiveToActionPerformer:ObjectTransferFromActiveCharacterPerformer
    {
        public override bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (!base.CanPerform(actionTemplate))
                return false;
            if (actionTemplate.TargetCharacter == null)
                return false;
            if (actionTemplate.TargetObject.StorageContainer.Owner.Name != actionTemplate.ActiveCharacter.Name)
                return false;
            if (actionTemplate.TargetCharacter.Inventory.MaximumCapacity <= actionTemplate.TargetCharacter.Inventory.Count)
                return false;
            return true;
        }

        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
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

        protected PerformActionResult PerformWithoutCheck(BaseActionTemplate actionTemplate)
        {
            var addedOk = actionTemplate.TargetCharacter.Inventory.Add(
    actionTemplate.TargetObject);
            return new PerformActionResult
            {
                ForAction = actionTemplate,
                ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
            };

        }
    }
}
