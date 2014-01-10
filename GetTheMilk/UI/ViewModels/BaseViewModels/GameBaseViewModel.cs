using System;

namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public abstract class GameBaseViewModel:ViewModelBase, IGameBaseViewModel
    {
        public abstract event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
