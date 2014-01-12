using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class Wall:IPositionableObject
    {
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }

        public Noun Name
        {
            get { return new Noun{Main="Wall",Narrator="wall"}; }
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
