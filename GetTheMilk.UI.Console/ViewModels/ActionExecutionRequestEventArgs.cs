using System;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class ActionExecutionRequestEventArgs:EventArgs
    {
        public BaseActionTemplate GameAction { get; private set; }
        public ActionExecutionRequestEventArgs(BaseActionTemplate action)
        {
            GameAction = action;
        }
    }
}
