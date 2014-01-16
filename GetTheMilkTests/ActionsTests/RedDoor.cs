using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilkTests.ActionsTests
{
    public class RedDoor : INonCharacterObject
    {
        public RedDoor()
        {
            Name = new Noun {Main = "RedDoor", Narrator = "Red Door"};
        }
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
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

        public Noun Name { get; private set; }
        public string ApproachingMessage { get; set; }
        public string CloseUpMessage { get; set; }
    }
}
