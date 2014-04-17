using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions
{
    public class Kill:TwoCharactersAction
    {

        public Kill()
        {
            Name = new Verb {Infinitive = "To Kill", Past = "killed", Present = "kill"};
            ActionType = ActionType.Kill;
            StartingAction = false;
            FinishTheInteractionOnExecution = true;
        }

        public override ActionResult Perform()
        {
            if(!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            ActiveCharacter.Experience += CalculationStrategies.CalculateWinExperience(ActiveCharacter, TargetCharacter);

            PileageCharacter(ActiveCharacter,TargetCharacter);
            if(TargetCharacter.StorageContainer!=null && TargetCharacter.StorageContainer.Owner!=null)
                TargetCharacter.StorageContainer.Remove(TargetCharacter as Character);
            if (TargetCharacter is IPlayer)
                ((Level) ActiveCharacter.StorageContainer.Owner).Player = null;
            TargetCharacter = null;
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok};
        }
        public override GameAction CreateNewInstance()
        {
            return new Kill();
        }

    }
}
