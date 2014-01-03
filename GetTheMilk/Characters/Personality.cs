using System.Collections.Generic;
using GetTheMilk.Actions.Interactions;

namespace GetTheMilk.Characters
{
    public class Personality
    {
        private SortedList<string, ActionReaction[]> _interactionRules;
        public PersonalityType Type { get; set; }

        public SortedList<string, ActionReaction[]> InteractionRules
        {
            get { return _interactionRules = (_interactionRules) ?? new SortedList<string, ActionReaction[]>(); }
        }
    }
}
