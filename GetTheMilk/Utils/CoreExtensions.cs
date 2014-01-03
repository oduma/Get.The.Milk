using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.Interactions;

namespace GetTheMilk.Utils
{
    public static class CoreExtensions
    {
        public static IEnumerable<ActionReaction> GetAllAplicableInteractionRules(this SortedList<string,ActionReaction[]> allRules,string forCharacter )
        {
            return ((!allRules.ContainsKey(GenericInteractionRulesKeys.All))?(new ActionReaction[0]):allRules[GenericInteractionRulesKeys.All])
                .Union((!allRules.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
                    ?(new ActionReaction[0]) : allRules[GenericInteractionRulesKeys.CharacterSpecific]).
                    Union((!allRules.ContainsKey(forCharacter))?(new ActionReaction[0]):allRules[forCharacter]);
        }
    }
}
