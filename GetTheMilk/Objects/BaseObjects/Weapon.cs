using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Objects.BaseObjects
{
    public abstract class Weapon : ITransactionalObject
    {
        public int Durability { get; set; }

        public WeaponType[] WeaponTypes { get; protected set; }
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public Noun Name { get; protected set; }
        public bool BlockMovement { get; protected set; }
        public virtual bool AllowsAction(GameAction a)
        {
            return (a is FightAction);
        }
        public virtual bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return ((a is Buy) || (a is Sell) || (a is GiveTo) || (a is TakeFrom)) && (o is ICharacter);
        }
        public Inventory StorageContainer { get; set; }
        public int BuyPrice { get; protected set; }
        public int SellPrice { get; protected set; }

        public int DefensePower { get; protected set; }

        public int AttackPower { get; protected set; }
    }
}
