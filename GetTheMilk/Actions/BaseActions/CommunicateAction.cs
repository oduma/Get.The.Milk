using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.BaseActions
{
    public class CommunicateAction : GameAction
    {
        public CommunicateAction()
        {
            Name = new Verb {Infinitive = "To Communicate", Past = "communicated", Present = "communicate"};
            ActionType = ActionType.Communicate;
        }

        public override ActionResult Perform()
        {
            return new ActionResult {ForAction = this, ExtraData = Message, ResultType = ActionResultType.Ok};
        }
        public string Message { get; set; }

        public override GameAction CreateNewInstance()
        {
            return new CommunicateAction();
        }


    }
}
