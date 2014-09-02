using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public abstract class BaseActionResponsePerformer:BaseHealthAffectingActionPerformer
    {
        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;
        
        protected PerformActionResult PerformResponseAction(BaseActionTemplate actionTemplate)
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

            if (baseAction.Category==CategorysCatalog.TwoCharactersCategory)
            {

                baseAction.CurrentPerformer.FeedbackFromSubAction -= ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).TwoCharactersActionFeedbackFromSubAction;
                baseAction.CurrentPerformer.FeedbackFromSubAction += ((TwoCharactersActionTemplatePerformer)baseAction.CurrentPerformer).TwoCharactersActionFeedbackFromSubAction;
            }
            return
                (baseAction.ActiveCharacter is IPlayer)?null:baseAction.ActiveCharacter.PerformAction(baseAction);
        }

        protected List<BaseActionTemplate> GetAvailableReactions(BaseActionTemplate actionTemplate)
        {
            IActionEnabled active;
            string targetName=(actionTemplate.TargetCharacter==null)?string.Empty:
                actionTemplate.TargetCharacter.Name.Main + "_Responses";
            if (actionTemplate.Category == typeof(ObjectResponseActionTemplate).Name)
            {
                active = actionTemplate.ActiveObject;
            }
            else
            {
                active = actionTemplate.ActiveCharacter;
            }
            if (active.Interactions == null)
                return null;
            if ((!active.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses) 
                || active.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null)
                && (!active.Interactions.ContainsKey(targetName)
                || active.Interactions[targetName].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null))
                return null;
            if (active.Interactions.ContainsKey(targetName) 
                && active.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
            {
                return active.Interactions[targetName].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).
                Union(active.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction)).ToList();
            }
            if (active.Interactions.ContainsKey(targetName))
            {
                return active.Interactions[targetName].Where(
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
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null))
            
                return null;

            return actionTemplate.ActiveCharacter.Interactions[targetMainName].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).ToList();
        }
    }
}
