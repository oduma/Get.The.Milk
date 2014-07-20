using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Utils;

namespace GetTheMilk.Objects.BaseObjects
{
    public class BaseActionEnabledObject:IActionEnabled
    {
        protected readonly List<IActionTemplatePerformer> _actionTemplatePerformers; 

        private Dictionary<string,BaseActionTemplate> _actions;

        public PerformActionResult PerformAction(BaseActionTemplate actionTemplate)
        {
            var performerInstance = FindPerformer(actionTemplate.PerformerType);
            if (performerInstance == null)
                return new PerformActionResult
                           {ForAction = actionTemplate, ResultType = ActionResultType.CannotPerformThisAction};
            return PerformActionWithPerformer(actionTemplate, performerInstance);
        }

        public virtual PerformActionResult PerformActionWithPerformer(BaseActionTemplate actionTemplate,
                                                               IActionTemplatePerformer performerInstance)
        {
            if (actionTemplate.Category == CategorysCatalog.OneObjectCategory)
            {
                return ((IOneObjectActionTemplatePerformer) performerInstance).Perform((OneObjectActionTemplate) actionTemplate);
            }
            if (actionTemplate.Category == CategorysCatalog.ObjectUseOnObjectCategory)
            {
                return
                    ((IObjectUseOnObjectActionTemplatePerformer) performerInstance).Perform(
                        (ObjectUseOnObjectActionTemplate) actionTemplate);
            }
            return new PerformActionResult
                       {ForAction = actionTemplate, ResultType = ActionResultType.CannotPerformThisAction};
        }

        public T CreateNewInstanceOfAction<T>(string uniqueId) where T : BaseActionTemplate
        {
            if (AllActions == null || !AllActions.ContainsKey(uniqueId) || AllActions[uniqueId].GetType()!=typeof(T))
                return null;
            return AllActions[uniqueId].Clone() as T;
        }

        public BaseActionTemplate CreateNewInstanceOfAction(string uniqueId)
        {
            if (AllActions == null || !AllActions.ContainsKey(uniqueId))
                return null;
            return AllActions[uniqueId].Clone();
        }

        public Dictionary<string,BaseActionTemplate> AllActions
        {
            get
            {
                _actions = (_actions) ?? new Dictionary<string,BaseActionTemplate>();
                return PickActionsFromInteractions();
            }
        }
        private Dictionary<string,BaseActionTemplate> PickActionsFromInteractions()
        {
            if(Interactions==null)
                return _actions;
            var result = _actions;
            
            foreach(var interactionKey in Interactions.Keys.Where(k=>
                k==GenericInteractionRulesKeys.CharacterSpecific || 
                k==GenericInteractionRulesKeys.AnyCharacterResponses || 
                k==GenericInteractionRulesKeys.AnyCharacter|| 
                k==GenericInteractionRulesKeys.All))
                    foreach (var interactionChar in Interactions[interactionKey].Select(a => a.Reaction))
                        if(!result.ContainsKey(interactionChar.Name.UniqueId))
                            result.Add(interactionChar.Name.UniqueId,interactionChar);
            foreach(var interactionKey in Interactions.Keys.Except(new string[]
                                                                       {
                                                                           GenericInteractionRulesKeys.AnyCharacter,
                                                                           GenericInteractionRulesKeys.AnyCharacterResponses,
                                                                           GenericInteractionRulesKeys.CharacterSpecific,
                                                                           GenericInteractionRulesKeys.PlayerResponses
                                                                       }))
                foreach (var interactionAll in Interactions[interactionKey].Select(a => a.Action))
                    if(!result.ContainsKey(interactionAll.Name.UniqueId))
                        result.Add(interactionAll.Name.UniqueId,interactionAll);

            return result;
        }

        protected Dictionary<string,BaseActionTemplate> AllActionsExcludeInteractions()
        {
            return _actions;
        }

        public SortedList<string, Interaction[]> Interactions { get; set; }

        public IActionTemplatePerformer FindPerformer(Type performerType)
        {
            var templatePerformer = _actionTemplatePerformers.FirstOrDefault(p => p.GetType()== performerType);
            if (templatePerformer != null)
                return templatePerformer;
            return null;
        }

        public virtual bool CanPerformAction(BaseActionTemplate actionTemplate)
        {
            var performerInstance = FindPerformer(actionTemplate.PerformerType);
            if (performerInstance == null)
                return false;
            if (actionTemplate.Category == CategorysCatalog.OneObjectCategory)
            {
                return ((IOneObjectActionTemplatePerformer)performerInstance).CanPerform((OneObjectActionTemplate)actionTemplate);
            }
            if (actionTemplate.Category == CategorysCatalog.ObjectUseOnObjectCategory)
            {
                return ((IObjectUseOnObjectActionTemplatePerformer)performerInstance).CanPerform((ObjectUseOnObjectActionTemplate)actionTemplate);
            }
            return false;
        }

        public void AddAvailableAction(BaseActionTemplate baseActionTemplate)
        {
            if(!_actions.ContainsKey(baseActionTemplate.Name.UniqueId))
                _actions.Add(baseActionTemplate.Name.UniqueId,baseActionTemplate);
        }


        public BaseActionEnabledObject()
        {
            _actionTemplatePerformers= new List<IActionTemplatePerformer>();

            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<IOneObjectActionTemplatePerformer>());
            _actionTemplatePerformers.AddRange(TemplatedActionPerformersFactory.GetFactory().GetAllActionPerformers<IObjectUseOnObjectActionTemplatePerformer>());
        }
    }
}
