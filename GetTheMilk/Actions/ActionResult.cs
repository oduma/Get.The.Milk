using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions
{
    public class ActionResult
    {
        public ActionResultType ResultType { get; set; }

        public object ExtraData { get; set; }

        public GameAction ForAction { get; set; }
    }
}
