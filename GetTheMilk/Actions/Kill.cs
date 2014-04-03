using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Kill:TwoCharactersAction
    {

        public Kill()
        {
            Name = new Verb {Infinitive = "To Kill", Past = "killed", Present = "kill"};
            ActionType = ActionType.Kill;
            StartingAction = false;

        }
        public double ExperienceTaken { get; set; }

        public override ActionResult Perform()
        {
            if(!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            ActiveCharacter.Experience += (int)Math.Ceiling(TargetCharacter.Experience*ExperienceTaken);
            if(TargetCharacter.StorageContainer!=null && TargetCharacter.StorageContainer.Owner!=null)
                TargetCharacter.StorageContainer.Remove(TargetCharacter as Character);
            TargetCharacter = null;
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok};
        }
        public override GameAction CreateNewInstance()
        {
            return new Kill();
        }

    }
}
