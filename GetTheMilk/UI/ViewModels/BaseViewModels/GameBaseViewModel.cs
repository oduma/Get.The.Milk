using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public abstract class GameBaseViewModel:ViewModelBase, IGameBaseViewModel
    {
        public event EventHandler<GameStartRequestEventArgs> GameStartRequest;

        public event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;

        public void FireStartRequestEvent(object sender, GameStartRequestEventArgs args)
        {
            if (GameStartRequest != null)
            {
                GameStartRequest(sender, args);
            }

        }


        public void FireAdvanceRequestEvent(object sender, GameAdvanceRequestEventArgs args)
        {
            if (GameAdvanceRequest != null)
            {
                GameAdvanceRequest(sender, args);
            }
        }
    }
}
