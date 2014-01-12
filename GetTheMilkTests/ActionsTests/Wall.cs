using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class Wall:IPositionableObject
    {
        public Wall()
        {
            Name = new Noun {Main = "Wall"};
        }
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }

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

        public Noun Name { get; private set; }
    }
}
