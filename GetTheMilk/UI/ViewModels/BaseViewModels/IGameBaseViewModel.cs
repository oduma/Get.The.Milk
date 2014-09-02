using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{

    public interface IGameBaseViewModel
    {
        event EventHandler<GameStartRequestEventArgs> GameStartRequest;

        void FireStartRequestEvent(object sender, GameStartRequestEventArgs args);

        event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;

        void FireAdvanceRequestEvent(object sender, GameAdvanceRequestEventArgs args);

    }
}
