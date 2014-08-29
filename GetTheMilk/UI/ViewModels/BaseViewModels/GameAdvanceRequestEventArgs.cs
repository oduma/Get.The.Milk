using System;
namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public class GameAdvanceRequestEventArgs:EventArgs
    {
        public RpgGameCore Game { get; private set; }

        public string Message { get; private set; }

        public string ActionName { get; private set; }

        public GameAdvanceRequestEventArgs(RpgGameCore game, string message, string actionName)
        {
            Game = game;
            Message = message;
            ActionName = actionName;
        }
    }
}
