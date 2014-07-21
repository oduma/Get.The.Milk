using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class CommunicateActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override bool CanPerform(TwoCharactersActionTemplate actionTemplate)
        {
            return base.CanPerform(actionTemplate) && !string.IsNullOrEmpty(actionTemplate.Message);
        }
    }
}
