using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Keep:OneObjectAction
    {

        public Keep()
        {
            Name = new Verb {Infinitive = "To Keep", Past = "kept", Present = "keep"};
        }
        public override void Perform(ICharacter c, IPositionableObject o)
        {
            if (o is Tool)
                c.ToolInventory.Add(o as Tool);
            else if (o is Weapon)
                c.WeaponInventory.Add(o as Weapon);
        }
    }
}
