using System.Collections.Generic;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using Newtonsoft.Json;

namespace GetTheMilk.Common
{
    public interface IActionEnabled
    {

        PerformActionResult PerformAction(BaseActionTemplate actionTemplate);

        T CreateNewInstanceOfAction<T>(string uniqueId) where T : BaseActionTemplate;

        BaseActionTemplate CreateNewInstanceOfAction(string uniqueId);

        [JsonIgnore]
        Dictionary<string,BaseActionTemplate> AllActions { get;}

        [JsonIgnore]
        SortedList<string, Interaction[]> Interactions { get; set; }

        bool CanPerformAction(BaseActionTemplate actionTemplate);

        void AddAvailableAction(BaseActionTemplate baseActionTemplate);
    }
}
