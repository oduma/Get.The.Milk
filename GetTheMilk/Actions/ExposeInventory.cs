using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class ExposeInventory:GameAction
    {
        public override bool CanPerform()
        {
            return ActiveCharacter.AllowsAction(this) && TargetCharacter.AllowsIndirectAction(this,ActiveCharacter);
        }

        public ExposeInventory()
        {
            Name = new Verb {Infinitive = "To Expose", Past = "exposed", Present = "expose"};
            ActionType = ActionType.ExposeInventory;
            StartingAction = false;

        }

        [LevelBuilderAccesibleProperty(typeof(ActionType))]
        public ActionType FinishActionType { get; set; }

        public bool IncludeWallet { get; set; }
        [LevelBuilderAccesibleProperty(typeof(bool))]
        public bool SelfInventory { get; set; }

        public override ActionResult Perform()
        {
            var result = new ActionResult {ResultType = ActionResultType.Ok, ForAction=this};
            var tempTargetCharacter = TargetCharacter;
            if (SelfInventory)
                TargetCharacter = ActiveCharacter;
            if (!CanPerform())
            {
                result.ResultType = ActionResultType.NotOk;
                return result;
            }
            var actionsFactory = ActionsFactory.GetFactory();
            result.ExtraData = new ExposeInventoryExtraData
                               {
                                   Contents =
                                       ActiveCharacter.Inventory.Select(
                                           o =>
                                       new ObjectWithPossibleActions
                                       {
                                           Object = o,
                                           PossibleUsses =(ActiveCharacter.ObjectTypeId!=tempTargetCharacter.ObjectTypeId)?
                                               DeterminePossibleUssesForObjectInInventory(o,
                                                   actionsFactory, tempTargetCharacter).ToArray():null
                                       }).ToArray(),
                                       FinishingAction=GetFinishingAction(actionsFactory, tempTargetCharacter),
                                   Money = (IncludeWallet) ? ActiveCharacter.Walet.CurrentCapacity : 0
                               };

            return result;
        }

        private GameAction GetFinishingAction(ActionsFactory actionsFactory, ICharacter alternateTargetCharacter)
        {
            var newAction =
                actionsFactory.GetActions()
                    .FirstOrDefault(
                        a =>
                            a.ActionType ==
                            ((FinishActionType == ActionType.Default)
                                ? BaseActions.ActionType.CloseInventory
                                : FinishActionType))
                    .CreateNewInstance();
            newAction.ActiveCharacter = TargetCharacter;
            newAction.FinishTheInteractionOnExecution = true;
            if (newAction is TwoCharactersAction)
                newAction.TargetCharacter = alternateTargetCharacter;
            else
                newAction.TargetCharacter = ActiveCharacter;
            return newAction;
        }


        private IEnumerable<GameAction> DeterminePossibleUssesForObjectInInventory(
            NonCharacterObject nonCharacterObject, 
            ActionsFactory actionsFactory,
            ICharacter alternateTargetCharacter)
        {
            if (!SelfInventory)
            {
                if (TargetCharacter is IPlayer)
                {
                    foreach (
                        var action in
                            actionsFactory.GetActions()
                                .Where(a => a is ObjectTransferFromAction))
                    {
                        var newAction = action.CreateNewInstance();
                        newAction.TargetObject = nonCharacterObject;
                        newAction.ActiveCharacter = TargetCharacter;
                        newAction.TargetCharacter = ActiveCharacter;
                        if(newAction.CanPerform())
                            yield return newAction;
                    }
                }
                
            }
            else
            {
                if (ActiveCharacter is IPlayer)
                {
                    foreach (
                        var action in
                            actionsFactory.GetActions()
                                .Where(a => a.ActionType == ActionType.SelectAttackWeapon || a.ActionType == ActionType.SelectDefenseWeapon))
                    {
                        var newAction = action.CreateNewInstance();
                        newAction.TargetObject = nonCharacterObject;
                        newAction.ActiveCharacter = TargetCharacter;
                        if(newAction.CanPerform())
                            yield return newAction;
                    }
                }
            }
        }


        public override GameAction CreateNewInstance()
        {
            return new ExposeInventory();
        }

    }
}
