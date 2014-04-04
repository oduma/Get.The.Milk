using System;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public GameAction GameAction { get; private set; }
        public ActionExecutionRequestEventArgs(GameAction action)
        {
            GameAction = action;
        }
    }
}
