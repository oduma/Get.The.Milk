using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public abstract class BaseActionResponsePerformer<T> where T:BaseActionTemplate 
    {
        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;
        
        protected PerformActionResult PerformResponseAction(T actionTemplate)
        {
            var availableActions = GetAvailableActions(actionTemplate);
            if (availableActions == null)
                return null;
            if (FeedbackFromOriginalAction != null)
                FeedbackFromOriginalAction(this, new FeedbackEventArgs(new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.Ok }));
            int actionToRespond = CalculationStrategies.SelectAWeightedRandomTemplateAction(0, availableActions.Count - 1, actionTemplate.Name.UniqueId);
            BaseActionTemplate baseAction;
            if (actionTemplate.TargetCharacter == null)
            {
                baseAction=actionTemplate.TargetObject.CreateNewInstanceOfAction(availableActions[actionToRespond].Name.UniqueId);
                
                baseAction.TargetCharacter = actionTemplate.ActiveCharacter;
                baseAction.TargetObject = actionTemplate.ActiveObject;
                return baseAction.Perform();

            }
            baseAction = actionTemplate.TargetCharacter.CreateNewInstanceOfAction(availableActions[actionToRespond].Name.UniqueId);

            baseAction.TargetCharacter = actionTemplate.ActiveCharacter;
            baseAction.ActiveObject = actionTemplate.TargetObject;
            baseAction.TargetObject = actionTemplate.ActiveObject;
            baseAction.ActiveCharacter = actionTemplate.TargetCharacter;

            if (baseAction.CurrentPerformer is ITwoCharactersActionTemplatePerformer)
            {

                ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).FeedbackFromSubAction -= ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).TwoCharactersActionFeedbackFromSubAction;
                ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).FeedbackFromSubAction += ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).TwoCharactersActionFeedbackFromSubAction;
            }
            return
                (baseAction.ActiveCharacter is IPlayer)?null:baseAction.ActiveCharacter.PerformAction(baseAction);
        }

        protected List<BaseActionTemplate> GetAvailableReactions(BaseActionTemplate actionTemplate)
        {
            IActionEnabled active;

            if(actionTemplate.Category==typeof(ObjectResponseActionTemplate).Name)
                active = actionTemplate.ActiveObject;
            else
                active = actionTemplate.ActiveCharacter;
            if (active.Interactions == null)
                return null;
            if ((!active.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses) 
                || active.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null)
                && (!active.Interactions.ContainsKey(GenericInteractionRulesKeys.All)
                || active.Interactions[GenericInteractionRulesKeys.All].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null))
                return null;
            if (active.Interactions.ContainsKey(GenericInteractionRulesKeys.All) 
                && active.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                return active.Interactions[GenericInteractionRulesKeys.All].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).
                Union(active.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction)).ToList();
            }
            else if (active.Interactions.ContainsKey(GenericInteractionRulesKeys.All))
            {
                return active.Interactions[GenericInteractionRulesKeys.All].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).ToList();
            }
            return active.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).ToList();
        }

        protected List<BaseActionTemplate> GetAvailableActions(BaseActionTemplate actionTemplate)
        {
            IActionEnabled target;
            string targetMainName;
            if(actionTemplate.Category==typeof(TwoCharactersActionTemplate).Name)
            {
                target = actionTemplate.TargetCharacter;
                targetMainName = actionTemplate.TargetCharacter.Name.Main;
            }
            else
            {
                target = actionTemplate.TargetObject;
                targetMainName = actionTemplate.TargetObject.Name.Main;

            }
            if (target == null)
                return null;
            if ((!actionTemplate.ActiveCharacter.Interactions.ContainsKey(targetMainName)
                || actionTemplate.ActiveCharacter.Interactions[targetMainName].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null) &&
                (!actionTemplate.ActiveCharacter.Interactions.ContainsKey(GenericInteractionRulesKeys.All)
                    || actionTemplate.ActiveCharacter.Interactions[GenericInteractionRulesKeys.All].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null))
            
                return null;

            return (!actionTemplate.ActiveCharacter.Interactions.ContainsKey(targetMainName))
                ?actionTemplate.ActiveCharacter.Interactions[GenericInteractionRulesKeys.All].
                Where(a=>a.Action.Equals(actionTemplate) && a.Reaction!=null).Select(a=>a.Reaction).ToList():
                actionTemplate.ActiveCharacter.Interactions[GenericInteractionRulesKeys.All].Where(a=>a.Action.Equals(actionTemplate) && a.Reaction!=null).Select(a=>a.Reaction).Union(
                actionTemplate.ActiveCharacter.Interactions[targetMainName].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction)).ToList();
        }
    }
}
