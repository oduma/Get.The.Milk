using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class TwoCharactersActionTemplate : BaseActionTemplate
    {
        public TwoCharactersActionTemplate()
        {
            Category = GetType().Name;
        }

        [LevelBuilderAccesibleProperty(typeof(string))]
        public string Message { get; set; }

        public override BaseActionTemplate Clone()
        {
            return new TwoCharactersActionTemplate { Name = Name, StartingAction = StartingAction, 
                FinishTheInteractionOnExecution = FinishTheInteractionOnExecution,Message=Message };
        }

        protected override object[] Translate()
        {
            
            var result= base.Translate();
            result[0] = (!string.IsNullOrEmpty(Message)) ? (result[0] + " " + Message + " to") : result[0];
            result[2] = (TargetCharacter == null) ? "Target Character Not Assigned" : TargetCharacter.Name.Narrator;

            return result;
        }

        public override bool Equals(object obj)
        {
            var result= base.Equals(obj);
            if (!result)
                return false;
            var y = (TwoCharactersActionTemplate)obj;
            if (y.PerformerType == typeof(CommunicateActionPerformer) && PerformerType == typeof(CommunicateActionPerformer))
                return y.Message == Message;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
