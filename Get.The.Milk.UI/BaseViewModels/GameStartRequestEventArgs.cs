using System;
using GetTheMilk.GameLevels;

namespace Get.The.Milk.UI.BaseViewModels
{
    public class GameStartRequestEventArgs:EventArgs
    {
        public RpgGameCore Game { get; private set; }

        public GameStartRequestEventArgs(RpgGameCore game)
        {
            Game = game;
        }
    }
}
