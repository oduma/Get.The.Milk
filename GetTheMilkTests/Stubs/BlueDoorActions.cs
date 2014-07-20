using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;

namespace GetTheMilkTests.Stubs
{
    public class BlueDoorActions:DecorActions
    {
        public BlueDoorActions()
        {
            ObjectTypeId = "BlueDoor";
        }
        public override bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return true;
        }

        public override bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return true;
        }
    }
}
