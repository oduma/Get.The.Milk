using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class OneObjectActionTemplatePerformer:BaseActionResponsePerformer<OneObjectActionTemplate>,IOneObjectActionTemplatePerformer
    {
        public virtual bool CanPerform(OneObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.TargetObject == null)
                return false;
            return (actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter)
            && actionTemplate.ActiveCharacter.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.TargetObject));

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
                           ExtraData = GetAvailableActions(actionTemplate)
                       };

        }

        protected void EstablishInteractionRules(OneObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter is IPlayer)
                ((IPlayer)actionTemplate.ActiveCharacter).LoadInteractions(actionTemplate.TargetObject,actionTemplate.TargetObject.Name.Main);
        }

        public string Category { get { return CategorysCatalog.OneObjectCategory; } }
    }
}
