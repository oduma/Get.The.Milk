using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.BaseActions
{
    public class CommunicateAction : GameAction
    {
        public CommunicateAction()
        {
            Name = new Verb {Infinitive = "To Communicate", Past = "communicated", Present = "communicate"};
        }

        public string Message { get; set; }
    }
}
