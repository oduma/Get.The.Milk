using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests
{
    public class ExplodePerformer:OneObjectActionTemplatePerformer
    {
        public override PerformActionResult Perform(Actions.ActionTemplates.OneObjectActionTemplate actionTemplate)
        {
            return base.Perform(actionTemplate);
        }
    }
}
