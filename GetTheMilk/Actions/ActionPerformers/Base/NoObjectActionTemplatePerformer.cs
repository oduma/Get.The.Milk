using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class NoObjectActionTemplatePerformer:INoObjectActionTemplatePerformer
    {
        public string Category { get { return CategorysCatalog.NoObjectCategory; } }
        public bool CanPerform(NoObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate);
        }

        public PerformActionResult Perform(NoObjectActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
                return new PerformActionResult {ForAction = actionTemplate, ResultType = ActionResultType.Ok};
            return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };
        }
    }
}
