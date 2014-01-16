using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class Knife:Weapon
    {
        private string _approachingMessage;
        private string _closeUpMessage;

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
            _approachingMessage = "In the distance a knife smiles as you.";
            _closeUpMessage = "It is a small but very sharp knife.";
        }

        public override string ApproachingMessage
        {
            get { return _approachingMessage; }
            set { _approachingMessage = value; }
        }

        public override string CloseUpMessage
        {
            get { return _closeUpMessage; }
            set { _closeUpMessage = value; }
        }
    }
}
