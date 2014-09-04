using GetTheMilk.GameLevels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetTheMilk.UI.ViewModels
{
    public class TitleViewModel
    {
        public string Title { get { return GameSettings.GetInstance().DefaultGameName; } }

        public string ContinueText { get { return "Continue"; } }
    }
}
