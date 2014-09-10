using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Controls
{
    public class ActionSelectedEventArgs:EventArgs
    {
        public BaseActionTemplate Action { get; private set; }

        public ActionSelectedEventArgs(BaseActionTemplate action)
        {
            Action = action;
        }
    }
}
