using GetTheMilk.Common;

namespace GetTheMilk.Objects.BaseObjects
{
    public class Weapon : Tool
    {
        public int Durability { get; set; }
        public WeaponType[] WeaponTypes { get; set; }
        public int DefensePower { get; set; }
        public int AttackPower { get; set; }
        public bool IsCurrentAttack { get; set; }
        public bool IsCurrentDefense { get; set; }

        public Weapon()
        {
            ObjectCategory = ObjectCategory.Weapon;
        }
    }
}
