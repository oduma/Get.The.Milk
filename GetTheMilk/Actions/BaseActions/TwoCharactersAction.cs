using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.BaseActions
{
    public class TwoCharactersAction : GameAction
    {
        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;

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
            if(availableActions[actionToRespond] is TwoCharactersAction)
            {
                ((TwoCharactersAction)availableActions[actionToRespond]).FeedbackFromSubAction -= TwoCharactersActionFeedbackFromSubAction;
                ((TwoCharactersAction)availableActions[actionToRespond]).FeedbackFromSubAction += TwoCharactersActionFeedbackFromSubAction;
            }
            return availableActions[actionToRespond].Perform();
        }

        void TwoCharactersActionFeedbackFromSubAction(object sender, FeedbackEventArgs e)
        {
            if (FeedbackFromSubAction != null)
                FeedbackFromSubAction(this, e);
        }

        protected ActionResult PerformWinLoseResponseAction(ActionResult result)
        {
            if (FeedbackFromOriginalAction != null)
                FeedbackFromOriginalAction(this, new FeedbackEventArgs(result));
            var availableAction = ((List<GameAction>) result.ExtraData)[0];
            return availableAction.Perform();
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

        protected void PileageCharacter(ICharacter pileager, ICharacter pileagee)
        {
            TakeFrom takeFrom = new TakeFrom();
            TakeMoneyFrom takeMoneyFrom = new TakeMoneyFrom();
            var pileageeInventory = pileagee.Inventory.ToList();
            ActionResult actionResult;
            foreach (var o in pileageeInventory)
            {
                if (pileager.Inventory.MaximumCapacity >= pileager.Inventory.Count)
                {
                    takeFrom.ActiveCharacter = pileager;
                    takeFrom.TargetCharacter = pileagee;
                    if (o.ObjectCategory == ObjectCategory.Weapon)
                    {
                        if (((Weapon)o).IsCurrentAttack)
                            pileagee.ActiveAttackWeapon = null;
                        if (((Weapon)o).IsCurrentDefense)
                            pileagee.ActiveDefenseWeapon = null;
                    }

                    takeFrom.TargetObject = o;
                    actionResult = takeFrom.Perform();
                    if(FeedbackFromSubAction!=null)
                        FeedbackFromSubAction(this,new FeedbackEventArgs(actionResult));
                }
                else
                {
                    break;
                }
            }
            takeMoneyFrom.ActiveCharacter = pileager;
            takeMoneyFrom.TargetCharacter = pileagee;
            takeMoneyFrom.Amount = (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity >
                                    pileagee.Walet.CurrentCapacity)
                ? pileagee.Walet.CurrentCapacity
                : (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity);
            actionResult = takeMoneyFrom.Perform();
            if(FeedbackFromSubAction!=null)
                FeedbackFromSubAction(this,new FeedbackEventArgs(actionResult));
        }

        public override GameAction CreateNewInstance()
        {
            return new TwoCharactersAction();
        }

    }
}
