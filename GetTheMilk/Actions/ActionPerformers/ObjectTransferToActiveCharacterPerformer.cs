using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers
{
    //default transfer from level to character
    public class ObjectTransferToActiveCharacterPerformer : ObjectTransferActionTemplatePerformer
    {
        public override bool CanPerform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (!base.CanPerform(actionTemplate))
                return false;
            if (actionTemplate.ActiveCharacter.Inventory.MaximumCapacity <= actionTemplate.ActiveCharacter.Inventory.Count)
                return false;
            return true;
        }

        public override PerformActionResult Perform(ActionTemplates.ObjectTransferActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
            {
                var addedOk = actionTemplate.ActiveCharacter.Inventory.Add(actionTemplate.TargetObject);
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
