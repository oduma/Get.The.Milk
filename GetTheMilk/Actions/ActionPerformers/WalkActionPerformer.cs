using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Navigation;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class WalkActionPerformer : MovementActionTemplatePerformer
    {
        public override bool CanPerform(BaseActionTemplate act)
        {
            MovementActionTemplate actionTemplate;
            if (!act.TryConvertTo(out actionTemplate))
                return false;
            return (base.CanPerform(actionTemplate) && actionTemplate.Direction != Direction.None);
        }
    }
}
