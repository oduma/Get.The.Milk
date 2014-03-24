using System.Collections.Generic;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class MovementActionExtraData
    {
        public IEnumerable<NonCharacterObject> ObjectsBlocking { get; set; }

        public IEnumerable<Character> CharactersBlocking { get; set; }

        public IEnumerable<Character> CharactersInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInRange { get; set; }

        public IEnumerable<NonCharacterObject> ObjectsInCell { get; set; }
    }
}
