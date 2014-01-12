using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class Knife:Weapon
    {
        public Knife()
        {
            Durability = 1000;
            AttackPower = 4;
            DefensePower = 1;
            BlockMovement = false;
            WeaponTypes=new WeaponType[]{WeaponType.Attack,WeaponType.Deffense};
            BuyPrice = 10;
            SellPrice = 3;
            Name= new Noun { Main = "Knife" };
        }
    }
}
