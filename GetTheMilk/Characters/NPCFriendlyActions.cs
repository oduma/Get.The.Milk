using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Characters
{
    public class NPCFriendlyActions: IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public bool AllowsAction(GameAction a)
        {
            return true;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return (!(a is Attack || a is Quit || a is InitiateHostilities || a is TakeFrom || a is TakeMoneyFrom));
        }
        public NPCFriendlyActions()
        {
            ObjectTypeId = "NPCFriendly";
            ObjectCategory=ObjectCategory.Character;
        }
    }
}
