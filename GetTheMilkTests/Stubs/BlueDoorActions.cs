using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilkTests.Stubs
{
    public class BlueDoorActions:IActionAllowed
    {
        public BlueDoorActions()
        {
            ObjectTypeId = "BlueDoor";
            ObjectCategory = ObjectCategory.Decor;
        }
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public bool AllowsAction(GameAction a)
        {
            return true;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return true;
        }
    }
}
