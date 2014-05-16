using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class AllowPass:TwoCharactersAction
    {
                public AllowPass()
        {
            Name = new Verb {Infinitive = "To Allow Pass", Past = "allowed to pass", Present = "allow pass"};
            ActionType = ActionType.AllowPass;
            StartingAction = false;
            FinishTheInteractionOnExecution = true;
        }

        public override ActionResult Perform()
        {
            if (!CanPerform())
                return new ActionResult {ForAction = this, ResultType = ActionResultType.NotOk};
            ActiveCharacter.BlockMovement = false;
            return new ActionResult {ResultType = ActionResultType.Ok,ForAction=this};
        }

        public override GameAction CreateNewInstance()
        {
            return new AllowPass();
        }



    }
}
