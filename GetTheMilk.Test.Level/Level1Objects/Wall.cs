using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class Wall:INonCharacterObject
    {
        private string _approachingMessage="You see a wall";

        private string _closeUpMessage="The wall is solid stone, unpassable for sure.";
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

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return false;
        }

        public Inventory StorageContainer { get; set; }

        public bool BlockMovement
        {
            get { return true; }
        }

        public string ApproachingMessage
        {
            get { return _approachingMessage; }
            set { _approachingMessage = value; }
        }

        public string CloseUpMessage
        {
            get { return _closeUpMessage; }
            set { _closeUpMessage = value; }
        }

    }
}
