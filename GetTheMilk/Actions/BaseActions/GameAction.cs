using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.BaseActions
{
    public class GameAction
    {
        public Verb Name { get; protected set; }

        public ActionType ActionType { get; protected set; }

        public GameAction()
        {
            ActionType = ActionType.Default;
        }
    }
}
