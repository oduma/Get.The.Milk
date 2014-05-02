﻿using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Objects
{
    public class DecorActions:IActionAllowed
    {
        public string ObjectTypeId { get; set; }
        public ObjectCategory ObjectCategory { get; set; }

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
            ObjectCategory = ObjectCategory.Decor;
        }
    }
}
