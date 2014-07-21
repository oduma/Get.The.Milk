using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.Objects
{
    public class KeyActions:ToolActions
    {
        public override bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return (a.Name.UniqueId=="Open");
        }
        public KeyActions()
        {
            ObjectTypeId = "Key";
        }
    }
}
