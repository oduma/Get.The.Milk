using System;
using System.Collections.Generic;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.ActionTemplates
{
    public class MovementActionTemplateExtraData
    {
        public IEnumerable<NonCharacterObject> ObjectsBlocking { get; set; }

        public IEnumerable<Character> CharactersBlocking { get; set; }

        public IEnumerable<Character> CharactersInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInCell { get; set; }

        public List<BaseActionTemplate> AvailableActionTemplates { get; set; }
    }
}
