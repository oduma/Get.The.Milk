using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Quit : TwoCharactersAction
    {
        public Quit()
        {
            Name = new Verb {Infinitive = "To Quit", Past = "quited", Present = "quit"};
            ActionType = ActionType.Quit;
            StartingAction = false;
        }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };
            if (ActiveCharacter is IPlayer)
                return PerformResponseAction(ActionType);
            return new ActionResult {ResultType = ActionResultType.RequestQuit,ForAction=this,ExtraData=GetAvailableActions()};
        }

        public override GameAction CreateNewInstance()
        {
            return new Quit();
        }

    }
}
