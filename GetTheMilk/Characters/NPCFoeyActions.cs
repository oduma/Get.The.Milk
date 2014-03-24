using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.UI.Translators.Common;

namespace GetTheMilk.Characters
{
    public class NPCFoeyActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return (a.ActionType==ActionType.Attack || a.ActionType==ActionType.Quit);
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
