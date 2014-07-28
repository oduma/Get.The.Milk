using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.NewActions.Tests
{
    public class ExplodePerformer:OneObjectActionTemplatePerformer
    {
        public override bool CanPerform(Actions.ActionTemplates.OneObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.TargetObject == null && actionTemplate.TargetCharacter==null)
                return false;
            if(!actionTemplate.ActiveObject.AllowsTemplateAction(actionTemplate))
                return false;
            return true;

        }

        public override PerformActionResult Perform(Actions.ActionTemplates.OneObjectActionTemplate actionTemplate)
        {
            return base.Perform(actionTemplate);
        }
    }
}
