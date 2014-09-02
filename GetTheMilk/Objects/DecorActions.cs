using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;

namespace GetTheMilk.Objects
{
    public class DecorActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public virtual bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return false;
        }

        public virtual bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return false;
        }
        public DecorActions()
        {
            ObjectTypeId = "Decor";
            ObjectCategory = ObjectCategory.Decor;
        }
    }
}
