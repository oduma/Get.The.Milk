using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;

namespace GetTheMilk.UI.Translators.Common
{
    public class MessagesForActionResult
    {
        public ActionResultType ResultType { get; set; }

        public List<Message> Messages { get; set; }

    }
}
