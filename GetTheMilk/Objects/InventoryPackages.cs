using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTheMilk.Objects
{
    public class InventoryPackages
    {
        public Inventory Contents { get; set; }
        public InventoryType InventoryType { get; set; }
        public int MaximumCapacity { get; set; }
    }
}
