using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Objects.BaseObjects
{
    public abstract class Tool : ITransactionalObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public abstract string Name { get; }
        public bool BlockMovement { get; protected set; }
        public virtual bool AllowsAction(GameAction action)
        {
            return true;
        }
        public virtual bool AllowsIndirectAction(GameAction action, IPositionableObject pObject)
        {
            return true;
        }
        public Inventory StorageContainer { get; set; }
        public int BuyPrice { get; protected set; }
        public int SellPrice { get; protected set; }
    }
}
