using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Objects.BaseObjects
{
    public interface IPositionableObject:IInventoryOwner
    {
        int MapNumber { get; set; }
        int CellNumber { get; set; }
        bool BlockMovement { get; }
        bool AllowsAction(GameAction action);
        bool AllowsIndirectAction(GameAction a, IPositionableObject o);
        Inventory StorageContainer{ get; set; }
    }
}
