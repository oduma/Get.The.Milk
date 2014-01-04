using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class Window:IPositionableObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }

        public string Name
        {
            get { return "Window"; }
        }

        public bool AllowsAction(GameAction a)
        {
                return !(a is Kick);
        }

        public bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return false;
        }

        public bool BlockMovement
        {
            get { return true; }
        }

        public Inventory StorageContainer { get; set; }
    }
}
