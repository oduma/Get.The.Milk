using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Objects.BaseObjects
{
    public abstract class Tool : ITransactionalObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public Noun Name { get; protected set; }

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
        public abstract string ApproachingMessage { get; set; }
        public abstract string CloseUpMessage { get; set; }
    }
}
