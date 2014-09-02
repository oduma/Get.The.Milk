using System.Collections.Generic;
using GetTheMilk.Actions.ActionTemplates;
using Newtonsoft.Json;

namespace GetTheMilk.Common
{
    public interface IActionEnabledCharacter
    {
        T CreateNewInstanceOfActionOnExposedContent<T>(string actionPool, string uniqueId) where T : BaseActionTemplate;

        [JsonIgnore]
        SortedList<string, IEnumerable<BaseActionTemplate>> ActionsForExposedContents { get; set; }

    }
}
