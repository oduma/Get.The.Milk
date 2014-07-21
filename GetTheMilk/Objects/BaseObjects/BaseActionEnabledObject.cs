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
        private Dictionary<string,BaseActionTemplate> _actions;

        public PerformActionResult PerformAction(BaseActionTemplate actionTemplate)
        {
            return actionTemplate.Perform();
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

        public virtual bool CanPerformAction(BaseActionTemplate actionTemplate)
        {
            return actionTemplate.CanPerform();
        }


        public void AddAvailableAction(BaseActionTemplate baseActionTemplate)
        {
            if (!_actions.ContainsKey(baseActionTemplate.Name.UniqueId))
                _actions.Add(baseActionTemplate.Name.UniqueId, baseActionTemplate);
        }
    }
}