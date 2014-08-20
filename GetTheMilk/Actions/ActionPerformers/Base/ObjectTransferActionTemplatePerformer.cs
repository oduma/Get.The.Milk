using System;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class ObjectTransferActionTemplatePerformer:IActionTemplatePerformer
    {
        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }

        public virtual bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.TargetObject == null)
                return false;
            if (actionTemplate.TargetCharacter != null) //Object transfer between two characters
            {
                if (!actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter)
                    || !actionTemplate.TargetCharacter.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.TargetObject))
                    return false;
                return true;
            }
            if (!actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter)
                || !actionTemplate.ActiveCharacter.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.TargetObject))
                return false;
            return true;
        }

        public virtual PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            throw new NotImplementedException();
        }


        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;


        public string Category
        {
            get { return CategorysCatalog.ObjectTransferCategory; }
        }
    }
}
