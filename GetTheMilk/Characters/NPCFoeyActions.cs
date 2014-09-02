using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;

namespace GetTheMilk.Characters
{
    public class NPCFoeyActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public bool AllowsTemplateAction(BaseActionTemplate a)
        {
            return (a.Name.UniqueId=="Attack"
                || a.Name.UniqueId == "Quit"
                || a.Name.UniqueId == "InitiateHostilities"
                || a.Name.UniqueId == "AcceptQuit"
                || a.Name.UniqueId == "TakeMoneyFrom");
        }

        public bool AllowsIndirectTemplateAction(BaseActionTemplate a, IPositionable o)
        {
            if (((a.GetType() == typeof(ExposeInventoryActionTemplate)) && ((ExposeInventoryActionTemplate)a).SelfInventory == false))
            {
                return false;
            }
            return true;
        }

        public NPCFoeyActions()
        {
            ObjectTypeId = "NPCFoe";
            ObjectCategory = ObjectCategory.Character;
        }
    }
}
