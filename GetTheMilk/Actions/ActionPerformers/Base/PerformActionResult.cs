using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class PerformActionResult
    {
        public ActionResultType ResultType { get; set; }

        public object ExtraData { get; set; }

        public BaseActionTemplate ForAction { get; set; }
    }
}
