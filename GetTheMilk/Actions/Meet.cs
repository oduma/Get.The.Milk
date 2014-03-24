using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Meet:TwoCharactersAction
    {
        public Meet()
        {
            Name = new Verb {Infinitive = "To Meet", Past = "meet", Present = "meet"};
            ActionType = ActionType.Meet;
        }
        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };

            EstablishInteractionRules();
            return new ActionResult {ForAction = this, ResultType = ActionResultType.Ok};
        }
        public override GameAction CreateNewInstance()
        {
            return new Meet();
        }

    }
}
