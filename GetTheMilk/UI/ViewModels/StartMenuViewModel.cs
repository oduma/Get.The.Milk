using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTheMilk.UI.ViewModels
{
    public class StartMenuViewModel
    {
        public string StartNew { get { return "New Game"; } }

        public string Load { get { return "Load"; } }

        public string Save { get { return "Save"; } }

        public string Exit { get { return "Exit"; } }
    }
}
