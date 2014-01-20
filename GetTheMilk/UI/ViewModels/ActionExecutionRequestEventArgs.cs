using System;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.UI.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public GameAction GameAction { get; private set; }

        public IPositionableObject TargetObject { get; private set; }

        public ActionExecutionRequestEventArgs(GameAction action,IPositionableObject targetObject=null)
        {
            GameAction = action;
            TargetObject = targetObject;
        }
    }
}
