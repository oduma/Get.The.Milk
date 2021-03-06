using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class RedDoorActions:DecorActions
    {
        public override bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return (a.Name.PerformerId=="Open" && o.Name.Main=="Red Key");
        }

        public RedDoorActions()
        {
            ObjectTypeId = "RedDoor";
        }
    }
}
