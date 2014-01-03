namespace GetTheMilk.Actions.BaseActions
{
    public class CommunicateAction : GameAction
    {
        public override string Name
        {
            get { return "Communicate"; }
        }

        public string Message { get; set; }
    }
}
