using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{

    public interface IGameBaseViewModel
    {
        event EventHandler<GameStartRequestEventArgs> GameStartRequest;

    }
}
