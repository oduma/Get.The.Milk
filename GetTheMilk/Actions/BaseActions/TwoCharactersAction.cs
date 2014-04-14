using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.BaseActions
{
    public class TwoCharactersAction : GameAction
    {
        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public override bool CanPerform()
        {
            return ActiveCharacter.AllowsAction(this) 
                && ((TargetCharacter!=null) 
                && TargetCharacter.AllowsIndirectAction(this, ActiveCharacter));
        }

        protected void EstablishInteractionRules()
        {
            if (ActiveCharacter is IPlayer)
                ((IPlayer)ActiveCharacter).LoadInteractionsWithPlayer(TargetCharacter);
            else if (TargetCharacter is IPlayer)
                ((IPlayer)TargetCharacter).LoadInteractionsWithPlayer(ActiveCharacter);

        }

        protected ActionResult PerformResponseAction(ActionType actionType)
        {
            if (FeedbackFromOriginalAction != null)
                FeedbackFromOriginalAction(this, new FeedbackEventArgs(new ActionResult { ForAction = this, ResultType = ActionResultType.Ok }));
            var availableActions = GetAvailableActions();
            int actionToRespond = CalculationStrategies.SelectAWeightedRandomAction(0, availableActions.Count - 1, actionType);
            availableActions[actionToRespond].TargetCharacter = ActiveCharacter;
            availableActions[actionToRespond].ActiveCharacter = TargetCharacter;
            return availableActions[actionToRespond].Perform();
        }
        protected List<GameAction> GetAvailableActions()
        {
            if (TargetCharacter is IPlayer)
            {
                var availableActions =
                    TargetCharacter.InteractionRules[ActiveCharacter.Name.Main].Union(
                        TargetCharacter.InteractionRules[GenericInteractionRulesKeys.All])
                        .Where(a => IsTheSameAction(a.Action))
                        .Select(a => a.Reaction).ToList();
                availableActions.ForEach(a=> { a.ActiveCharacter = TargetCharacter;
                                                 a.TargetCharacter = ActiveCharacter;
                });
                return availableActions;
            }

            return TargetCharacter.InteractionRules[GenericInteractionRulesKeys.CharacterSpecific].Union(TargetCharacter.InteractionRules[GenericInteractionRulesKeys.All]).Where(
                a => IsTheSameAction(a.Action)).Select(a => a.Reaction).ToList();
        }

        private bool IsTheSameAction(GameAction action)
        {
            if (action.ActionType == ActionType.Communicate && ActionType == ActionType.Communicate)
                return ((Communicate) action).Message == ((Communicate) this).Message;
            return action.ActionType == ActionType;

        }


        public override GameAction CreateNewInstance()
        {
            return new TwoCharactersAction();
        }

    }
}
