using System.Collections.Generic;
using GetTheMilk.UI.Translators.Common;
using GetTheMilk.UI.Translators.MovementResultTemplates;

namespace GetTheMilk.Settings
{
    public class TemplatesPackage
    {
        public List<MessagesFor> MessagesFor { get; set; }

        public MovementExtraDataTemplate MovementExtraDataTemplate { get; set; }
    }
}
