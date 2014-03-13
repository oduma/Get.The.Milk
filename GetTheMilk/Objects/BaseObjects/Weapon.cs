namespace GetTheMilk.Objects.BaseObjects
{
    public class Weapon : Tool, IWeapon
    {
        public int Durability { get; set; }
        public WeaponType[] WeaponTypes { get; set; }
        public int DefensePower { get; set; }
        public int AttackPower { get; set; }

        public Weapon()
        {
            ObjectCategory = ObjectCategory.Weapon;
        }
    }
}
