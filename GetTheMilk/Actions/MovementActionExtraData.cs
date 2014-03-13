using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class MovementActionExtraData
    {
        public NonCharacterObject[] ObjectsBlocking { get; set; }

        public Character[] CharactersBlocking { get; set; }

        public Character[] CharactersInRange { get; set; }

        public int MoveToCell { get; set; }

        public NonCharacterObject[] ObjectsInRange { get; set; }

        public NonCharacterObject[] ObjectsInCell { get; set; }
    }
}
