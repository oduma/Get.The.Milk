using System.Collections.Generic;
using GetTheMilk.UI.Translators.Common;
using GetTheMilk.UI.Translators.MovementResultTemplates;

namespace GetTheMilk.Settings
{
    public class TemplatesPackage
    {
        public List<MessagesForActionResult> MessagesForActionResult { get; set; }

        public MovementExtraDataTemplate MovementExtraDataTemplate { get; set; }

        public List<Message> ActionTypeMessages { get; set; } 
    }
}
