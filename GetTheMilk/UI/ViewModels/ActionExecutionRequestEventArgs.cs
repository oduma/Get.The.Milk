using System;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public GameAction GameAction { get; private set; }
        public ActionExecutionRequestEventArgs(GameAction action,bool returnToActionView)
        {
            GameAction = action;
            ReturnToActionView = returnToActionView;
        }

        public bool ReturnToActionView   { get; private set; }
    }
}
