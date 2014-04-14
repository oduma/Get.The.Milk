using System;

namespace GetTheMilk.Actions.BaseActions
{
    public class FeedbackEventArgs:EventArgs
    {
        public ActionResult ActionResult { get; private set; }

        public FeedbackEventArgs(ActionResult actionResult)
        {
            ActionResult = actionResult;
        }
    }
}
