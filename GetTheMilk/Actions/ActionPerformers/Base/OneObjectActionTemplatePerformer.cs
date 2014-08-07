using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class OneObjectActionTemplatePerformer:BaseActionResponsePerformer<OneObjectActionTemplate>,IOneObjectActionTemplatePerformer
    {

        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }

        public virtual bool CanPerform(OneObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.TargetObject == null)
                return false;
            return (actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter)
            && actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate));

        }

        public virtual PerformActionResult Perform(OneObjectActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult { ResultType = ActionResultType.NotOk, ForAction = actionTemplate };
            return (PerformResponseAction(actionTemplate)) ??
                   new PerformActionResult
                       {
                           ForAction = actionTemplate,
                           ResultType = ActionResultType.Ok,
                           ExtraData = GetAvailableReactions(actionTemplate)
                       };

        }
    }
}
