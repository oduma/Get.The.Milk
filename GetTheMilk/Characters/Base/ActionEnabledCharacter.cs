using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects.Base;
using GetTheMilk.Utils;

namespace GetTheMilk.Characters.Base
{
    public class ActionEnabledCharacter:BaseActionEnabledObject,IActionEnabledCharacter
    {
        protected override void AssignExecutor(BaseActionTemplate act)
        {
            act.ActiveCharacter = (Character)this;
        }

        public SortedList<string, IEnumerable<BaseActionTemplate>> ActionsForExposedContents { get; set; }

        public ActionEnabledCharacter()
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