using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;

namespace GetTheMilk.Characters
{
    public class PlayerActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public bool AllowsTemplateAction(BaseActionTemplate a)
        {
            if (((a.GetType() == typeof(ExposeInventoryActionTemplate)) && ((ExposeInventoryActionTemplate)a).SelfInventory == false))
            {
                return false;
            }
            return true;
        }

        public bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            return true;
        }

        public PlayerActions()
        {
            ObjectTypeId = "Player";
            ObjectCategory = ObjectCategory.Player;
        }
    }
}
