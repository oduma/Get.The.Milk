using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Test.Level.Level1Objects
{
    public class RedDoor : INonCharacterObject
    {
        private string _approachingMessage="There is a red door in the distance, or is it a wall?";
        private string _closeUpMessage="Upclose you realise it is a door and it seems to be locked.";
        public int MapNumber { get; set; }
        public int CellNumber { get; set; }
        public Noun Name { get { return new Noun {Main = "Red Door",Narrator="red door"}; } }
        public bool AllowsAction(GameAction a) { return false; }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (a is Open && o is RedKey);
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
