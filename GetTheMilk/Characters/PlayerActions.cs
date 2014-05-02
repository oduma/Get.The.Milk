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
    public class PlayerActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

        public bool AllowsAction(GameAction a)
        {
            return true;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
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
