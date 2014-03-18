using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilkTests.Stubs
{
    public class RedDoorActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return false;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (a is Open && o.Name.Main=="Red Key");
        }
        public RedDoorActions()
        {
            ObjectTypeId = "RedDoor";
        }
    }
}