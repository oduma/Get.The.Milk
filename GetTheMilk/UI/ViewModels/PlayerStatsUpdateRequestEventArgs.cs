using System;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerStatsUpdateRequestEventArgs:EventArgs
    {
        public PerformActionResult ActionResult { get; private set; }

        public PlayerStatsUpdateRequestEventArgs(PerformActionResult actionResult)
        {
            ActionResult = actionResult;
        }
    }
}
