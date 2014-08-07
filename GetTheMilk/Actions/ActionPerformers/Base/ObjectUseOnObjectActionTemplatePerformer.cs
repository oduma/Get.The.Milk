using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class ObjectUseOnObjectActionTemplatePerformer:BaseActionResponsePerformer<ObjectUseOnObjectActionTemplate>,IObjectUseOnObjectActionTemplatePerformer
    {
        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }

        public bool CanPerform(ObjectUseOnObjectActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter==null || actionTemplate.ActiveObject == null || actionTemplate.TargetObject == null)
                return false;
            return (actionTemplate.ActiveObject.AllowsTemplateAction(actionTemplate) &&
                    actionTemplate.TargetObject.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveObject));

        }

        public PerformActionResult Perform(ObjectUseOnObjectActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult {ForAction = actionTemplate, ResultType = ActionResultType.NotOk};

            bool success = (actionTemplate.ChanceOfSuccess == ChanceOfSuccess.Full) ||
                           CalculationStrategies.CalculateSuccessOrFailure(actionTemplate.ChanceOfSuccess,
                                                                           actionTemplate.ActiveCharacter.Experience);
            if (actionTemplate.DestroyActiveObject && success)
            {
                if (actionTemplate.ActiveObject.StorageContainer != null)
                    actionTemplate.ActiveObject.StorageContainer.Remove(actionTemplate.ActiveObject);
                actionTemplate.ActiveObject = null;
            }
            if (actionTemplate.DestroyTargetObject && success)
            {
                if (actionTemplate.TargetObject.StorageContainer != null)
                    actionTemplate.TargetObject.StorageContainer.Remove(actionTemplate.TargetObject);
                actionTemplate.TargetObject = null;
            }
            if (!success)
            {
                actionTemplate.ActiveCharacter.Health -= (actionTemplate.ActiveCharacter.Health * actionTemplate.PercentOfHealthFailurePenalty / 100);
            }

            if(actionTemplate.TargetObject==null)
                return new PerformActionResult { ForAction = actionTemplate, 
                    ResultType = (success) ? ActionResultType.Ok : ActionResultType.NotOk};

            return (PerformResponseAction(actionTemplate))??new PerformActionResult { ForAction = actionTemplate, 
                    ResultType = (success) ? ActionResultType.Ok : ActionResultType.NotOk,
                    ExtraData=GetAvailableReactions(actionTemplate) };
        }
    }
}
