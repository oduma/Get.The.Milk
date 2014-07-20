using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class CommunicateActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override bool CanPerform(ActionTemplates.TwoCharactersActionTemplate actionTemplate)
        {
            return base.CanPerform(actionTemplate) && !string.IsNullOrEmpty(actionTemplate.Message);
        }
    }
}
