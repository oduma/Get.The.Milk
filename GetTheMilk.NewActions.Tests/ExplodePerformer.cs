using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests
{
    public class ExplodePerformer:OneObjectActionTemplatePerformer
    {
        public override PerformActionResult Perform(Actions.ActionTemplates.BaseActionTemplate actionTemplate)
        {
            return base.Perform(actionTemplate);
        }
    }
}
