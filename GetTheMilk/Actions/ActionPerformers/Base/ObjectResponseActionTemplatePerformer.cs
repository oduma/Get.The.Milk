using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class ObjectResponseActionTemplatePerformer:BaseActionResponsePerformer, IActionTemplatePerformer
    {
        public bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveObject == null)
                return false;
            return actionTemplate.ActiveObject.AllowsTemplateAction(actionTemplate);
        }

        public virtual PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            if (CanPerform(actionTemplate))
                return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.Ok,
                    ExtraData=GetAvailableReactions(actionTemplate) };
            return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };
        }

        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }


        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;


        public string Category
        {
            get { return CategorysCatalog.ObjectResponseCategory; }
        }
    }
}
