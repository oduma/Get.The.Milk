using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public class GameStartRequestEventArgs:EventArgs
    {
        public Game Game { get; private set; }

        public GameStartRequestEventArgs(Game game)
        {
            Game = game;
        }
    }
}
