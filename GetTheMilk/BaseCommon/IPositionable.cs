using System;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.BaseCommon
{
    public interface IPositionable:IInventoryOwner
    {
        int CellNumber { get; set; }

        bool BlockMovement { get; set; }
        
        string ObjectTypeId { get; set; }

        Func<GameAction, bool> AllowsAction { get; set; }

        Func<GameAction, IPositionable, bool> AllowsIndirectAction { get; set; }


    }
}
