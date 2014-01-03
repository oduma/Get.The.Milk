using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public abstract class OneObjectAction : GameAction
    {
        public abstract void Perform(ICharacter active, IPositionableObject pObject);
    }
}
