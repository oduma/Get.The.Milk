using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Settings;
using GetTheMilk.Utils;

namespace GetTheMilk.Characters.BaseCharacters
{
    public class ActionEnabledCharacter:BaseActionEnabledObject,IActionEnabledCharacter
    {
        public override PerformActionResult PerformActionWithPerformer(BaseActionTemplate actionTemplate, IActionTemplatePerformer performerInstance)
        {
            var result = base.PerformActionWithPerformer(actionTemplate, performerInstance);
            if (result.ResultType != ActionResultType.CannotPerformThisAction)
                return result;
            if (actionTemplate.Category == CategorysCatalog.ObjectTransferCategory)
                return
                    ((IObjectTransferActionTemplatePerformer)performerInstance).Perform(
                        (ObjectTransferActionTemplate)actionTemplate);
            if (actionTemplate.Category == CategorysCatalog.TwoCharactersCategory)
                return
                    ((ITwoCharactersActionTemplatePerformer)performerInstance).Perform(
                        (TwoCharactersActionTemplate)actionTemplate);
            if (actionTemplate.Category == CategorysCatalog.MovementCategory)
                return
                    ((IMovementActionTemplatePerformer)performerInstance).Perform(
                        (MovementActionTemplate)actionTemplate);
            if (actionTemplate.Category == CategorysCatalog.ExposeInventoryCategory)
                return
                    ((IExposeInventoryActionTemplatePerformer)performerInstance).Perform(
                        (ExposeInventoryActionTemplate)actionTemplate);
            if (actionTemplate.Category == CategorysCatalog.NoObjectCategory)
                return
                    ((INoObjectActionTemplatePerformer)performerInstance).Perform(
                        (NoObjectActionTemplate)actionTemplate);
            return result;
        }

        public override bool CanPerformAction(BaseActionTemplate actionTemplate)
        {
            var result = base.CanPerformAction(actionTemplate);
            if (result)
                return true;
            var performerInstance = FindPerformer(actionTemplate.PerformerType);
            if (performerInstance == null)
                return false;
            if (actionTemplate.Category == CategorysCatalog.ObjectTransferCategory)
            {
                return
                    ((IObjectTransferActionTemplatePerformer)performerInstance).CanPerform(
                        (ObjectTransferActionTemplate)actionTemplate);
            }
            if (actionTemplate.Category == CategorysCatalog.TwoCharactersCategory)
            {
                return
                    ((ITwoCharactersActionTemplatePerformer)performerInstance).CanPerform(
                        (TwoCharactersActionTemplate)actionTemplate);
            }
            if (actionTemplate.Category == CategorysCatalog.MovementCategory)
            {
                return
                    ((IMovementActionTemplatePerformer)performerInstance).CanPerform(
                        (MovementActionTemplate)actionTemplate);
            }
            if (actionTemplate.Category == CategorysCatalog.ExposeInventoryCategory)
            {
                return
                    ((IExposeInventoryActionTemplatePerformer) performerInstance).CanPerform(
                        (ExposeInventoryActionTemplate) actionTemplate);

            }
            if (actionTemplate.Category == CategorysCatalog.NoObjectCategory)
            {
                return
                    ((INoObjectActionTemplatePerformer)performerInstance).CanPerform(
                        (NoObjectActionTemplate)actionTemplate);

            }
            return false;
        }

        protected void LoadInteractionsForAll()
        {

            Interactions.Add(GenericInteractionRulesKeys.All,
                              new Interaction[]
                                  {
                                      new Interaction
                                          {
                                              Action =new TwoCharactersActionTemplate{PerformerType=typeof(AttackActionPerformer),Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},StartingAction=false},
                                              Reaction =new TwoCharactersActionTemplate{PerformerType=typeof(AttackActionPerformer),Name=new Verb{UniqueId= "Attack",Past="attacked",Present="attack"},StartingAction=false}
                                          },
                                      new Interaction
                                          {
                                              Action =new TwoCharactersActionTemplate{PerformerType=typeof(AttackActionPerformer),Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},StartingAction=false},
                                              Reaction=new TwoCharactersActionTemplate{PerformerType=typeof(TwoCharactersActionTemplatePerformer),Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},StartingAction=false}
                                          }
                                  });
        }

        public SortedList<string, IEnumerable<BaseActionTemplate>> ActionsForExposedContents { get; set; }

        public ActionEnabledCharacter():base()
        {
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<ITwoCharactersActionTemplatePerformer>()); 
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<IObjectTransferActionTemplatePerformer>());
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<IMovementActionTemplatePerformer>());
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers
                    <IExposeInventoryActionTemplatePerformer>());
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers
                    <INoObjectActionTemplatePerformer>());
            ActionsForExposedContents = (ActionsForExposedContents) ??
                                        new SortedList<string, IEnumerable<BaseActionTemplate>>
                                            {{ContentActionsKeys.SelfContentActions, GameSettings.GetInstance().SelfInventoryActions}};

        }

        public T CreateNewInstanceOfActionOnExposedContent<T>(string actionPool, string uniqueId) where T : BaseActionTemplate
        {
            if (ActionsForExposedContents == null || !ActionsForExposedContents.ContainsKey(actionPool))
                return null;
            var defaultActionTemplate =
                ActionsForExposedContents[actionPool].FirstOrDefault(
                    a => a.Name.UniqueId == uniqueId);
            return defaultActionTemplate != null ? defaultActionTemplate.Clone() as T : null;
        }
    }
}
