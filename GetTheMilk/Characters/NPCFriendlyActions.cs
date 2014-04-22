using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.UI.Translators.Common;

namespace GetTheMilk.Characters
{
    public class NPCFriendlyActions: IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return true;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return !(a is Attack || a is Quit || a is InitiateHostilities);
        }
        public NPCFriendlyActions()
        {
            ObjectTypeId = "NPCFriendly";
        }
    }
}
