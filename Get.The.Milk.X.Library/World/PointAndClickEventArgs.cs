using GetTheMilk.Actions.ActionTemplates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.World
{
    public class PointAndClickEventArgs:EventArgs
    {
        public IEnumerable<BaseActionTemplate> Actions { get; private set; }

        public Point ClickSource { get; private set; }

        public PointAndClickEventArgs(IEnumerable<BaseActionTemplate> actions, Point clickSource)
        {
            Actions = actions;
            ClickSource = clickSource;
        }
    }
}
