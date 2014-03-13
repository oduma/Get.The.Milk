﻿using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.Actions
{
    public class Teleport:MovementAction
    {
        public Teleport()
        {
            Name = new Verb {Infinitive = "To Teleport", Past = "teleported", Present = "teleport"};
            DefaultDistance = 0;
            ActionType = ActionType.Teleport;
        }
    }
}
