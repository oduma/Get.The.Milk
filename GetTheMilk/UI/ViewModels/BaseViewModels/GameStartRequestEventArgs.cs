using System;
using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
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
