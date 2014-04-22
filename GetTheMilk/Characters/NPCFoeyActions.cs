using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Characters
{
    public class NPCFoeyActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return (a.ActionType==ActionType.Attack 
                || a.ActionType==ActionType.Quit 
                || a.ActionType==ActionType.InitiateHostilities 
                || a.ActionType==ActionType.AcceptQuit
                || a.ActionType==ActionType.TakeMoneyFrom);
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return true;
        }

        public NPCFoeyActions()
        {
            ObjectTypeId = "NPCFoe";
        }
    }
}
