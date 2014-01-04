using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class RedDoor : IPositionableObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public string Name { get { return "Red Door"; } }
        public bool AllowsAction(GameAction a) { return false; }

        public bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return (a is Open && o is RedKey);
        }

        public Inventory StorageContainer { get; set; }

        public bool BlockMovement
        {
            get { return true; }
        }
    }
}