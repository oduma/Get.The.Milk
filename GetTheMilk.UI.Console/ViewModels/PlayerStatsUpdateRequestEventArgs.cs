using System;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.UI.Console.ViewModels
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
