using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class RedDoorActions:DecorActions
    {
        public override bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (a is Open && o.Name.Main=="Red Key");
        }

        public RedDoorActions()
        {
            ObjectTypeId = "RedDoor";
        }
    }
}
