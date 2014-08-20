using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class CommunicateActionPerformer:TwoCharactersActionTemplatePerformer
    {
        public override bool CanPerform(BaseActionTemplate act)
        {
            TwoCharactersActionTemplate actionTemplate;
            if (!act.TryConvertTo(out actionTemplate))
                return false;
            return base.CanPerform(actionTemplate) && !string.IsNullOrEmpty(actionTemplate.Message);
        }
    }
}
