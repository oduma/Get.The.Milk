using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class Communicate : TwoCharactersAction
    {
        public override bool CanPerform()
        {
            return (base.CanPerform()) && !string.IsNullOrEmpty(Message);
        }

        public Communicate()
        {
            Name = new Verb {Infinitive = "To Communicate", Past = "communicated", Present = "communicate"};
            ActionType = ActionType.Communicate;
            StartingAction = false;

        }

        public override ActionResult Perform()
        {
            if (!CanPerform())
            {
                return new ActionResult { ForAction = this, ResultType = ActionResultType.NotOk };
            }
            if (ActiveCharacter is IPlayer)
                return PerformResponseAction(ActionType);
            return new ActionResult {ForAction = this, ExtraData=GetAvailableActions(), ResultType = ActionResultType.Ok};
        }
        public string Message { get; set; }

        public override GameAction CreateNewInstance()
        {
            return new Communicate();
        }


    }
}
