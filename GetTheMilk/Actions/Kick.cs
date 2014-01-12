using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Kick:OneObjectAction
    {
        public Kick()
        {
            Name = new Verb {Infinitive = "To Kick", Past = "kicked", Present = "kick"};
        }
        public override void Perform(ICharacter c, IPositionableObject o)
        {
                o.StorageContainer = null;
        }
    }
}
