using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class Window:INonCharacterObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }

        public Window()
        {
            Name = new Noun {Main = "Window",Narrator="window"};
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
        public Noun Name { get; private set; }
        public string ApproachingMessage { get; set; }
        public string CloseUpMessage { get; set; }
    }
}
