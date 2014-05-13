using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.Interactions
{
    public class ActionReaction
    {
        public GameAction Action { get; set; }

        public GameAction Reaction { get; set; }

        public override string ToString()
        {
            string actionName= GetActionName(Action);
            string reactionName = GetActionName(Reaction);

            return actionName + " - " + reactionName;
        }

        private string GetActionName(GameAction action)
        {
            string actionName;
            switch (action.ActionType)
            {
                case ActionType.Communicate:
                    actionName = ((Communicate) action).Message;
                    break;
                case ActionType.Attack:
                    actionName = action.Name.Present + " " + action.TargetCharacter.Name.Main;
                    break;
                default:
                    actionName = action.Name.Present;
                    break;
            }
            return actionName;
        }
    }
}
