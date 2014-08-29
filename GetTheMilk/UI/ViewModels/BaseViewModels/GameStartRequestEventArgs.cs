using System;

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
