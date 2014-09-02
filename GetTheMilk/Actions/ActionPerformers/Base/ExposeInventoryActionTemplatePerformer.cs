using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class ExposeInventoryActionTemplatePerformer:IActionTemplatePerformer
    {
        public bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.TargetCharacter == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate) &&
                   actionTemplate.TargetCharacter.AllowsIndirectTemplateAction(actionTemplate, actionTemplate.ActiveCharacter);

        }

        public PerformActionResult Perform(BaseActionTemplate act)
        {
            ExposeInventoryActionTemplate actionTemplate;
            if (!act.TryConvertTo(out actionTemplate))
                return new PerformActionResult { ResultType = ActionResultType.NotOk, ForAction = act };
            var result = new PerformActionResult { ResultType = ActionResultType.Ok, ForAction = actionTemplate };
            var tempTargetCharacter = actionTemplate.TargetCharacter;
            if (actionTemplate.SelfInventory)
                actionTemplate.TargetCharacter = actionTemplate.ActiveCharacter;
            if (!CanPerform(actionTemplate))
            {
                result.ResultType = ActionResultType.NotOk;
                return result;
            }

            result.ExtraData = new InventoryExtraData
            {
                Contents =
                    actionTemplate.ActiveCharacter.Inventory.Select(
                        o =>
                    new ObjectWithPossibleActionTemplates
                    {
                        Object = o,
                        PossibleUsses = DeterminePossibleUssesForObjectInInventory(o,
                                actionTemplate).ToArray()
                    }).ToArray(),
                FinishingAction = GetFinishingAction(tempTargetCharacter,actionTemplate)
            };

            return result;
        }

        private BaseActionTemplate GetFinishingAction(Character alternateTargetCharacter, 
            ExposeInventoryActionTemplate actionTemplate)
        {
            var uniqueId = (string.IsNullOrEmpty(actionTemplate.FinishingAction.ToString()))
                                 ? "CloseInventory"
                                 : actionTemplate.FinishingAction.ToString();
            var templateAction = actionTemplate.TargetCharacter.CreateNewInstanceOfAction(uniqueId);
            if (templateAction == null)
                return null;
            templateAction.ActiveCharacter = actionTemplate.TargetCharacter;
            templateAction.TargetCharacter=(templateAction.GetType() == 
                typeof(TwoCharactersActionTemplate))?alternateTargetCharacter:
                templateAction.TargetCharacter = actionTemplate.ActiveCharacter;
            return templateAction;
        }


        private IEnumerable<BaseActionTemplate> DeterminePossibleUssesForObjectInInventory(
            NonCharacterObject nonCharacterObject,ExposeInventoryActionTemplate actionTemplate)
        {
            if(actionTemplate.TargetCharacter.ObjectTypeId=="Player")
            if (!actionTemplate.SelfInventory)
            {

                foreach(var actionForContent in actionTemplate.ActiveCharacter.ActionsForExposedContents[actionTemplate.ActiveCharacter.ObjectTypeId]
                    .Where(a=>a.GetType()==typeof(ObjectTransferActionTemplate)))
                {
                    var newAction =
                        actionTemplate.ActiveCharacter.CreateNewInstanceOfActionOnExposedContent
                            <ObjectTransferActionTemplate>(actionTemplate.ActiveCharacter.ObjectTypeId,
                                                           actionForContent.Name.UniqueId);
                    if (newAction != null)
                    {
                        newAction.TargetObject = nonCharacterObject;
                        newAction.ActiveCharacter = actionTemplate.TargetCharacter;
                        newAction.TargetCharacter = actionTemplate.ActiveCharacter;
                        if (newAction.ActiveCharacter.CanPerformAction(newAction))
                            yield return newAction;
                    }
                }
            }
            else
            {
                foreach(var actionForContent in actionTemplate.ActiveCharacter.ActionsForExposedContents["SelfContentActions"])
                {
                    var newAction =
                        actionTemplate.ActiveCharacter.CreateNewInstanceOfActionOnExposedContent
                            <OneObjectActionTemplate>(ContentActionsKeys.SelfContentActions,
                                                      actionForContent.Name.UniqueId);
                    if(newAction!=null)
                    {
                        newAction.TargetObject = nonCharacterObject;
                        newAction.ActiveCharacter = actionTemplate.TargetCharacter;
                        if (
                            newAction.ActiveCharacter.CanPerformAction(newAction))
                            yield return newAction;
                    }

                }
            }
        }

        public string PerformerType
        {
            get { return GetType().Name; }
        }


        public event EventHandler<FeedbackEventArgs> FeedbackFromOriginalAction;

        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;


        public string Category
        {
            get { return CategorysCatalog.ExposeInventoryCategory; }
        }
    }
}
