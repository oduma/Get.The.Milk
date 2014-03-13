using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.Translators.Common;

namespace GetTheMilk.Objects
{
    public class DecorActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public bool AllowsAction(GameAction a)
        {
            return false;
        }

        public bool AllowsIndirectAction(GameAction a, IPositionable o)
        {
            return false;
        }
        public DecorActions()
        {
            ObjectTypeId = "Decor";
        }
    }
}
