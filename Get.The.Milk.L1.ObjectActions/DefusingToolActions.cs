using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class DefusingToolActions:ToolActions
    {
        public new bool AllowsAction(GameAction a)
        {
            return (a.ActionType==ActionType.Defuse);
        }

        public DefusingToolActions()
        {
            ObjectTypeId = "DefusingTool";
            ObjectCategory = ObjectCategory.Tool;
        }
    }
}
