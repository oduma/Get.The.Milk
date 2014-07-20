using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests
{
    public class ExplodePerformer:OneObjectActionTemplatePerformer
    {
        public override bool CanPerform(Actions.ActionTemplates.OneObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.TargetObject == null)
                return false;
            return (actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter)
            && actionTemplate.ActiveObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.TargetObject));

        }

        public override PerformActionResult Perform(Actions.ActionTemplates.OneObjectActionTemplate actionTemplate)
        {
            return base.Perform(actionTemplate);
        }
    }
}
