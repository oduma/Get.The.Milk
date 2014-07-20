using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace Get.The.Milk.L1.ObjectActions
{
    public class ExplosiveActions:ToolActions
    {
        public override bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return (a.Name.PerformerId=="Explode");
        }

        public override bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return (base.AllowsIndirectTemplateAction(a, o)) ||(a.Name.PerformerId=="Defuse");
        }
        public ExplosiveActions()
        {
            ObjectTypeId = "Explosive";
            ObjectCategory = ObjectCategory.Tool;
        }
    }
}
