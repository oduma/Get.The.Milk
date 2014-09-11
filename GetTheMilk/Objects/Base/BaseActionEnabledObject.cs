using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Utils;

namespace GetTheMilk.Objects.Base
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
            var act= AllActions[uniqueId].Clone() as T;
            AssignExecutor(act);
            return act;
        }

        protected virtual void AssignExecutor(BaseActionTemplate act)
        {
            act.ActiveObject = (NonCharacterObject)this;
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
                k==GenericInteractionRulesKeys.AnyCharacter|| 
                k.EndsWith("_Responses")))
                    foreach (var interactionChar in Interactions[interactionKey].Where(a=>a.Reaction!=null).Select(a => a.Reaction))
                        if(!result.ContainsKey(interactionChar.Name.UniqueId))
                            result.Add(interactionChar.Name.UniqueId,interactionChar);
            foreach(var interactionKey in Interactions.Keys.Except(new[]
                                                                       {
                                                                           GenericInteractionRulesKeys.AnyCharacter
                                                                       }))
                foreach (var interactionAll in Interactions[interactionKey].Select(a => a.Action))
                    if(!result.ContainsKey(interactionAll.Name.UniqueId))
                        result.Add(interactionAll.Name.UniqueId,interactionAll);

            return result;
        }

        protected IEnumerable<BaseActionTemplate> AllActionsExcludeInteractions()
        {
            return _actions.Select(a=>a.Value);
        }

        public SortedList<string, Interaction[]> Interactions { get; set; }

        public virtual bool CanPerformAction(BaseActionTemplate actionTemplate)
        {
            return actionTemplate.CanPerform();
        }

        public virtual void AddAvailableAction(BaseActionTemplate baseActionTemplate)
        {
            baseActionTemplate.ActiveObject = (NonCharacterObject)this;
            AddAction(baseActionTemplate);
        }

        protected void AddAction(BaseActionTemplate baseActionTemplate)
        {
            if (_actions == null)
                _actions = new Dictionary<string, BaseActionTemplate>();
            if (!_actions.ContainsKey(baseActionTemplate.Name.UniqueId))
                _actions.Add(baseActionTemplate.Name.UniqueId, baseActionTemplate);

        }

        public string SpriteName
        {
            get;
            set;
        }
    }
}
