namespace GetTheMilk.Objects.BaseObjects
{
    public interface IWeapon
    {
        int Durability { get; set; }

        WeaponType[] WeaponTypes { get; set; }
        
        int DefensePower { get; set; }

        int AttackPower { get; set; }


    }
}
