using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    public class RequestScreenChangeEventArgs:EventArgs
    {
        public BaseGameState TargetScreen { get; private set; }

        public RequestScreenChangeEventArgs(BaseGameState targetScreen)
        {
            TargetScreen = targetScreen;
        }
    }
}
