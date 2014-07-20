using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Levels;

namespace GetTheMilk.Actions.ActionPerformers
{
    //default transfer to level
    public class ObjectTransferFromActiveCharacterPerformer:ObjectTransferActionTemplatePerformer
    {
        public override PerformActionResult Perform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
            {
                var addedOk = ((Level) actionTemplate.ActiveCharacter.StorageContainer.Owner).Inventory.Add(
                    actionTemplate.TargetObject);
                return new PerformActionResult
                {
                    ForAction = actionTemplate,
                    ResultType = (addedOk) ? ActionResultType.Ok : ActionResultType.NotOk
                };
            }
            return new PerformActionResult
            {
                ForAction = actionTemplate,
                ResultType = ActionResultType.NotOk
            };
        }
    }
}
