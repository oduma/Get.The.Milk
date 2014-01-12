using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class UseOnCharacter : GameAction
    {
        public IPositionableObject ObjectToUse
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Character CharacterToUseOn
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

    }
}
