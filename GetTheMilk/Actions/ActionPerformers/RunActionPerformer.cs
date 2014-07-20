using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class RunActionPerformer:MovementActionTemplatePerformer
    {
        public override bool CanPerform(ActionTemplates.MovementActionTemplate actionTemplate)
        {
            return (base.CanPerform(actionTemplate) && actionTemplate.Direction != Direction.None);
        }
    }
}
