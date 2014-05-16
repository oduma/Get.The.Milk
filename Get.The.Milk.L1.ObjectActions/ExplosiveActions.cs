using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class ExplosiveActions:ToolActions
    {
        public new bool AllowsAction(GameAction a)
        {
            return (a.ActionType==ActionType.Explode);
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (base.AllowsIndirectAction(a, o)) ||(a.ActionType==ActionType.Defuse);
        }
        public ExplosiveActions()
        {
            ObjectTypeId = "Explosive";
            ObjectCategory = ObjectCategory.Tool;
        }
    }
}
