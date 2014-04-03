using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions
{
    public class InventorySubAction:InventorySubActionType
    {
        public GameAction Action { get; set; }
    }
}
