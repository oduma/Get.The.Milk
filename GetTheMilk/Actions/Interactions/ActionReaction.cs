using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.Interactions
{
    public class ActionReaction
    {
        public GameAction Action { get; set; }

        public GameAction Reaction { get; set; }

        public override string ToString()
        {
            return Action.Name.Present + " - " + Reaction.Name.Present;
        }
    }
}
