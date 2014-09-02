using System;
using System.Windows;
using GetTheMilk.GameLevels;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class GameAdvanceViewModel:GameBaseViewModel
    {
 
        public RelayCommand PerformAction { get; private set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    RaisePropertyChanged("Message");
                }

            }
        }

        private string _actionName;

        public string ActionName
        {
            get { return _actionName; }
            set
            {
                if (value != _actionName)
                {
                    _actionName = value;
                    RaisePropertyChanged("ActionName");
                }

            }
        }

        private Visibility _actionVisible;

        public Visibility ActionVisible
        {
            get { return _actionVisible; }
            set
            {
                if (value != _actionVisible)
                {
                    _actionVisible = value;
                    RaisePropertyChanged("ActionVisible");
                }

            }
        }

        public RpgGameCore Game { get; set; }

        public GameAdvanceViewModel()
        {
            PerformAction = new RelayCommand(PerformActionCommand);
        }

        private void PerformActionCommand()
        {
            FireStartRequestEvent(this, new GameStartRequestEventArgs(Game));
        }
    }
}
