using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class Wall:IPositionableObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }

        public string Name
        {
            get { return "Wall"; }
        }

        public bool AllowsAction(GameAction a)
        {
            return false;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return false;
        }

        public Inventory StorageContainer { get; set; }

        public bool BlockMovement
        {
            get { return true; }
        }
    }
}
