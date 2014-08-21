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
            string actionName = GetActionName(Action);
            string reactionName = GetActionName(Reaction);

            return actionName + " - " + reactionName;
        }

        private string GetActionName(BaseActionTemplate action)
        {
            if (action==null || action.CurrentPerformer == null)
                return "no action";
            if(action.CurrentPerformer.GetType()==typeof(CommunicateActionPerformer))
                    return ((TwoCharactersActionTemplate)action).Message;
            if(action.CurrentPerformer.GetType()==typeof(AttackActionPerformer))
                    return action.Name.Present + " " + action.TargetCharacter.Name.Main;
            return action.Name.Present;
        }
    }
}
