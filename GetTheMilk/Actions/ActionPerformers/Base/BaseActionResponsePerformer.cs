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
            IActionTemplatePerformer performer;
            if (actionTemplate.TargetCharacter == null)
            {
                baseAction=actionTemplate.TargetObject.CreateNewInstanceOfAction(availableActions[actionToRespond].Name.UniqueId);
                
                baseAction.TargetCharacter = actionTemplate.ActiveCharacter;
                baseAction.ActiveObject = actionTemplate.TargetObject;
                baseAction.TargetObject = actionTemplate.ActiveObject;
                return baseAction.ActiveObject.PerformAction(availableActions[actionToRespond]);

            }
            baseAction = actionTemplate.TargetCharacter.CreateNewInstanceOfAction(availableActions[actionToRespond].Name.UniqueId);

            baseAction.TargetCharacter = actionTemplate.ActiveCharacter;
            baseAction.ActiveObject = actionTemplate.TargetObject;
            baseAction.TargetObject = actionTemplate.ActiveObject;
            baseAction.ActiveCharacter = actionTemplate.TargetCharacter;

            performer=baseAction.ActiveCharacter.FindPerformer(
            baseAction.PerformerType);
            if (performer is ITwoCharactersActionTemplatePerformer)
            {

                ((TwoCharactersActionTemplatePerformer)performer).FeedbackFromSubAction -= ((TwoCharactersActionTemplatePerformer)performer).TwoCharactersActionFeedbackFromSubAction;
                ((TwoCharactersActionTemplatePerformer)performer).FeedbackFromSubAction += ((TwoCharactersActionTemplatePerformer)performer).TwoCharactersActionFeedbackFromSubAction;
            }
            return
                baseAction.ActiveCharacter.PerformActionWithPerformer(baseAction, performer);
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
            if (actionTemplate.TargetCharacter.Interactions.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
                return result.Union(actionTemplate.TargetCharacter.Interactions[GenericInteractionRulesKeys.CharacterSpecific]).Where(
                a => a.Action.Equals(actionTemplate)).Select(a => a.Reaction).ToList();
            return result.Select(a => a.Reaction).ToList();
        }


    }
}
