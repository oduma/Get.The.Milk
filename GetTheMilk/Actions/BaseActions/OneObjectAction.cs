using System;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class OneObjectAction : GameAction
    {
        public virtual void Perform(ICharacter c, NonCharacterObject o)
        {
            throw new NotImplementedException();
        }
    }
}
