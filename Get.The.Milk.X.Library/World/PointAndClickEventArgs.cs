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

        public IEnumerable<string> Messages { get; private set; }

        public Point ClickSource { get; private set; }

        public PointAndClickEventArgs(IEnumerable<object>actionsAndMessages, Point clickSource)
        {
            Actions = actionsAndMessages.Where(o=>o.GetType()!=typeof(string)).Select(o=>(BaseActionTemplate)o);
            Messages = actionsAndMessages.Where(o => o.GetType() == typeof(string)).Select(o => (string)o);
            ClickSource = clickSource;
        }
    }
}
