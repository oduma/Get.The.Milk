using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;

namespace GetTheMilk.Characters
{
    public class NPCFriendlyActions: IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }
        public bool AllowsTemplateAction(BaseActionTemplate actionTemplate)
        {
            return true;
        }

        public bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            if (((a is ExposeInventoryActionTemplate) && ((ExposeInventoryActionTemplate)a).SelfInventory))
            {
                return false;
            }
            if (o!=null && o.ObjectTypeId == "NPCFoe")
                return true;
            return
                (!(a.Name.UniqueId == "Attack" || a.Name.UniqueId == "Quit" ||
                   a.Name.UniqueId == "InitiateHostilities" ||
                   a.Name.UniqueId == "TakeMoneyFrom"));
        }
        public NPCFriendlyActions()
        {
            ObjectTypeId = "NPCFriendly";
            ObjectCategory=ObjectCategory.Character;
        }
    }
}
