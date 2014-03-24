using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Quit : TwoCharactersAction
    {
        public Quit()
        {
            Name = new Verb {Infinitive = "To Quit", Past = "quited", Present = "quit"};
            ActionType = ActionType.Quit;
        }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            return new ActionResult {ResultType = ActionResultType.RequestQuit};
        }

        public override GameAction CreateNewInstance()
        {
            return new Quit();
        }

    }
}
