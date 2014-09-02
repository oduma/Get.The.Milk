using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;

namespace GetTheMilk.Objects
{
    public class ToolActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }
        public virtual bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return false;
        }

        public virtual bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return a.GetType() == typeof(ObjectTransferActionTemplate) && o is ICharacter;

        }
        public ToolActions()
        {
            ObjectTypeId = "Tool";
            ObjectCategory = ObjectCategory.Tool;

        }
    }
}
