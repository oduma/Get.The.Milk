using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Kick:OneObjectAction
    {
        public override string Name
        {
            get { return "Kick"; }
        }

        public override void Perform(ICharacter c, IPositionableObject o)
        {
                o.StorageContainer = null;
        }
    }
}
