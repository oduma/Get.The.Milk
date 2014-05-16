using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class TrapDoorActions:DecorActions
    {
        public override bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (a is Explode && o.ObjectTypeId=="Explosive");
        }

        public TrapDoorActions()
        {
            ObjectTypeId = "TrapDoor";
        }
    }
}
