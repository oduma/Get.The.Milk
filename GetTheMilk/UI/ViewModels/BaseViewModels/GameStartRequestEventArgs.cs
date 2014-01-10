using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public class GameStartRequestEventArgs:EventArgs
    {
        public int Level { get; private set; }

        public GameStartRequestEventArgs(int level)
        {
            Level = level;
        }
    }
}
