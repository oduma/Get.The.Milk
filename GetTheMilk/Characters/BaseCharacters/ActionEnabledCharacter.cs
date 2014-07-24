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
        protected override void AssignExecutor(BaseActionTemplate act)
        {
            act.ActiveCharacter = (Character)this;
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
                                              Action =new TwoCharactersActionTemplate{PerformerType= typeof(AttackActionPerformer),Name=new Verb{UniqueId="Attack",Past="attacked",Present="attack"},StartingAction=false},
                                              Reaction=new TwoCharactersActionTemplate{PerformerType=typeof(TwoCharactersActionTemplatePerformer),Name=new Verb{UniqueId="Quit",Past="quited",Present="quit"},StartingAction=false}
                                          }
                                  });
        }

        public SortedList<string, IEnumerable<BaseActionTemplate>> ActionsForExposedContents { get; set; }

        public ActionEnabledCharacter():base()
        {
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

        public override void AddAvailableAction(BaseActionTemplate baseActionTemplate)
        {
            baseActionTemplate.ActiveCharacter = (Character)this;
            AddAction(baseActionTemplate);
        }
    }
}
