using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

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
                baseAction.ActiveCharacter.PerformAction(baseAction);
        }

        protected List<BaseActionTemplate> GetAvailableActions(ObjectResponseActionTemplate actionTemplate)
        {
            if (!actionTemplate.ActiveObject.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses) 
                || actionTemplate.ActiveObject.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null)
                return null;
            return actionTemplate.ActiveObject.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).ToList();
        }

        protected List<BaseActionTemplate> GetAvailableActions(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.TargetObject == null ||
                !actionTemplate.ActiveCharacter.Interactions.ContainsKey(actionTemplate.TargetObject.Name.Main)
                || actionTemplate.ActiveCharacter.Interactions[actionTemplate.TargetObject.Name.Main].
                FirstOrDefault(ar => ar.Action.Equals(actionTemplate) && ar.Reaction != null) == null)
                return null;
            return actionTemplate.ActiveCharacter.Interactions[actionTemplate.TargetObject.Name.Main].Where(
                a => a.Action.Equals(actionTemplate) && a.Reaction != null).Select(a => a.Reaction).ToList();
        }

        protected List<BaseActionTemplate> GetAvailableActions(TwoCharactersActionTemplate actionTemplate)
        {
            if (actionTemplate.TargetCharacter is IPlayer)
            {
                var availableActions =
                    actionTemplate.TargetCharacter.Interactions[actionTemplate.ActiveCharacter.Name.Main].Union(
                        actionTemplate.TargetCharacter.Interactions[GenericInteractionRulesKeys.All])
                        .Where(a => a.Action.Equals(actionTemplate))
                        .Select(a => a.Reaction).ToList();
                availableActions.ForEach(a =>
                {
                    a.ActiveCharacter = actionTemplate.TargetCharacter;
                    a.TargetCharacter = actionTemplate.ActiveCharacter;
                });
                return availableActions;
            }
            var result = actionTemplate.TargetCharacter.Interactions[GenericInteractionRulesKeys.All];
            //if (actionTemplate.TargetCharacter.Interactions.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
            //    return result.Union(actionTemplate.TargetCharacter.Interactions[GenericInteractionRulesKeys.CharacterSpecific]).Where(
            //    a => a.Action.Equals(actionTemplate)).Select(a => a.Reaction).ToList();
            return result.Select(a => a.Reaction).ToList();
        }


    }
}
