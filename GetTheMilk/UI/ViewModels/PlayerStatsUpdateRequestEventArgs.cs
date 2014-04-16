using System;
using GetTheMilk.Actions;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerStatsUpdateRequestEventArgs:EventArgs
    {
        public ActionResult ActionResult { get; private set; }

        public PlayerStatsUpdateRequestEventArgs(ActionResult actionResult)
        {
            ActionResult = actionResult;
        }
    }
}
