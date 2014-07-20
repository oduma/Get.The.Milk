using System;
using GetTheMilk.Actions.ActionPerformers.Base;

namespace GetTheMilk.Actions.ActionPerformers
{
    public class FeedbackEventArgs:EventArgs
    {
        public PerformActionResult ActionResult { get; private set; }

        public FeedbackEventArgs(PerformActionResult actionResult)
        {
            ActionResult = actionResult;
        }
    }
}
