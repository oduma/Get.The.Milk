using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class AttackActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override PerformActionResult Perform(TwoCharactersActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };

            CalculationStrategies.CalculateDamages(actionTemplate.ActiveCharacter.PrepareAttackHit(), 
                actionTemplate.TargetCharacter.PrepareDefenseHit(), 
                actionTemplate.ActiveCharacter, 
                actionTemplate.TargetCharacter);
            return (PerformResponseAction(actionTemplate))??
            new PerformActionResult
                       {
                           ResultType = ActionResultType.Ok,
                           ForAction = actionTemplate,
                           ExtraData = GetAvailableReactions(actionTemplate)
                       };

        }


        private PerformActionResult FightConcluded(TwoCharactersActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter.Health > 0 && actionTemplate.TargetCharacter.Health > 0)
                return null;

            var winningCharacter = (actionTemplate.ActiveCharacter.Health>0)
                                      ? actionTemplate.ActiveCharacter
                                      : actionTemplate.TargetCharacter;
            var looserCharacter = (actionTemplate.ActiveCharacter.Health>0)
                                       ? actionTemplate.TargetCharacter
                                       : actionTemplate.ActiveCharacter;

            winningCharacter.Experience += CalculationStrategies.CalculateWinExperience(winningCharacter, looserCharacter);
            PileageCharacter(winningCharacter, looserCharacter);
            if (looserCharacter.StorageContainer != null && looserCharacter.StorageContainer.Owner != null)
                looserCharacter.StorageContainer.Remove(looserCharacter as Character);
            if(looserCharacter.ObjectTypeId=="Player")
                ((Level)winningCharacter.StorageContainer.Owner).Player = null;
            if(actionTemplate.ActiveCharacter.Health > 0)
                actionTemplate.TargetCharacter=null;
            else 
                actionTemplate.ActiveCharacter=null;

            return new PerformActionResult
            {
                ExtraData=winningCharacter,
                ResultType = ActionResultType.Win,
                ForAction = actionTemplate
            };
        }


    }
}
