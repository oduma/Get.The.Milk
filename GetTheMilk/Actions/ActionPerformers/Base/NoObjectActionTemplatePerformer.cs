using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class NoObjectActionTemplatePerformer:IActionTemplatePerformer
    {
        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }

        public bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate);
        }

        public PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
                return new PerformActionResult {ForAction = actionTemplate, ResultType = ActionResultType.Ok};
            return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };
        }


        public event System.EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public event System.EventHandler<FeedbackEventArgs> FeedbackFromSubAction;


        public string Category
        {
            get { return CategorysCatalog.NoObjectCategory; }
        }
    }
}
