using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class UseOnCharacter : GameAction
    {
        public IPositionable ObjectToUse
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public ICharacter CharacterToUseOn
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
