using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.Interactions
{
    public class Interaction
    {
        public BaseActionTemplate Action { get; set; }

        public BaseActionTemplate Reaction { get; set; }

        public override string ToString()
        {
            return ((Action==null)?"no action" : Action.ToString()) + " - " + ((Reaction==null)?"no action" : Reaction.ToString());
        }
    }
}
