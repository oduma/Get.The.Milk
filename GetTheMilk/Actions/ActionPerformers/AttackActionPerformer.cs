using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class AttackActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };
            CalculationStrategies.CalculateDamages(actionTemplate.ActiveCharacter.PrepareAttackHit(), 
                actionTemplate.TargetCharacter.PrepareDefenseHit(), 
                actionTemplate.ActiveCharacter, 
                actionTemplate.TargetCharacter);
            if(NotifyDeath(actionTemplate,PileageCharacter))
                return new PerformActionResult { ResultType = ActionResultType.Ok, ForAction = actionTemplate };

            return (PerformResponseAction(actionTemplate))??
            new PerformActionResult
                       {
                           ResultType = ActionResultType.Ok,
                           ForAction = actionTemplate,
                           ExtraData = GetAvailableReactions(actionTemplate)
                       };

        }
    }
}
