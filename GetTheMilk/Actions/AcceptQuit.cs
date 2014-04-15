using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class AcceptQuit:TwoCharactersAction
    {
                public AcceptQuit()
        {
            Name = new Verb { Infinitive = "To Accept Quit", Past = "accepted quit", Present = "accept quit" };
            ActionType = ActionType.AcceptQuit;
                    StartingAction = false;
        }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};

            ActiveCharacter.Experience += (int)(TargetCharacter.Experience*0.25);
            TargetCharacter.Experience -= (int) (TargetCharacter.Experience*0.25);
            if (ActiveCharacter is IPlayer)
                return PerformResponseAction(ActionType);
            PileageCharacter(ActiveCharacter, TargetCharacter);
            return new ActionResult {ResultType = ActionResultType.Ok,ForAction=this,ExtraData=GetAvailableActions()};
        }

        public override GameAction CreateNewInstance()
        {
            return new AcceptQuit();
        }


    }
}
