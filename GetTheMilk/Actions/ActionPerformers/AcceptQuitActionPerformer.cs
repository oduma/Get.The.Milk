using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class AcceptQuitActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            var result = base.Perform(actionTemplate);
            if (result.ResultType == ActionResultType.Ok)
            {
                actionTemplate.ActiveCharacter.Experience += (int) (actionTemplate.TargetCharacter.Experience*0.25);
                actionTemplate.TargetCharacter.Experience -= (int) (actionTemplate.TargetCharacter.Experience*0.25);
            }
            return result;
        }

    }
}
