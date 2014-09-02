using System;

namespace Get.The.Milk.UI.BaseViewModels
{

    public interface IGameBaseViewModel
    {
        event EventHandler<GameStartRequestEventArgs> GameStartRequest;

        void FireStartRequestEvent(object sender, GameStartRequestEventArgs args);

        event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;

        void FireAdvanceRequestEvent(object sender, GameAdvanceRequestEventArgs args);

    }
}