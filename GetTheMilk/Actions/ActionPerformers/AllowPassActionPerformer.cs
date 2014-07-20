using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class AllowPassActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(TwoCharactersActionTemplate actionTemplate)
        {
            var result = base.Perform(actionTemplate);
            if (result.ResultType == ActionResultType.Ok)
                actionTemplate.ActiveCharacter.BlockMovement = false;
            return result;
        }
    }
}
