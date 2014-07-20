using System;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class ObjectTransferActionTemplatePerformer:IObjectTransferActionTemplatePerformer
    {
        public virtual bool CanPerform(ObjectTransferActionTemplate actionTemplate)
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

        public virtual PerformActionResult Perform(ObjectTransferActionTemplate actionTemplate)
        {
            throw new NotImplementedException();
        }

        public string Category { get { return CategorysCatalog.ObjectTransferCategory; } }
    }
}
