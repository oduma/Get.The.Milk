using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Keep:OneObjectAction
    {
        public override string Name
        {
            get { return "Keep"; }
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
