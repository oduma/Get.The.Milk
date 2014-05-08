using System;
using GetTheMilk.Levels;
using GetTheMilk.Settings;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerSetupViewModel:GameBaseViewModel
    {
        private int _maximumAvailableBonusPoints;

        public RelayCommand SaveAndStart { get; private set; }

        public PlayerSetupViewModel()
        {
            Description = "Player setup:";
            MaximumAvailableBonusPoints = GameSettings.GetInstance().MaximumAvailableBonusPoints;
            Money = MaximumAvailableBonusPoints/2;
            SaveAndStart= new RelayCommand(StartNewGame);
        }

        private void StartNewGame()
        {
            var game = Game.CreateGameInstance();
            game.Player.SetPlayerName(Name);
            game.Player.Walet.CurrentCapacity = Money;
            game.Player.Experience = Experience;
            game.Player.Health = GameSettings.GetInstance().FullDefaultHealth;
            if (GameStartRequest != null)
            {
                GameStartRequest(this,new GameStartRequestEventArgs(game));
            }

        }

        public int Experience
        {
            get { return _experience; }
            set
            {
                if (value != _experience)
                {
                    _experience = value;
                    Money = MaximumAvailableBonusPoints - _experience;
                    RaisePropertyChanged("Experience");
                }
            }
        }

        public int Money
        {
            get { return _money; }
            set
            {
                if (value != _money)
                {
                    _money = value;
                    Experience = MaximumAvailableBonusPoints - _money;
                    RaisePropertyChanged("Money");
                }
            }
        }

        public int MaximumAvailableBonusPoints
        {
            get { return _maximumAvailableBonusPoints; }
            set
            {
                if (value != _maximumAvailableBonusPoints)
                {
                    _maximumAvailableBonusPoints = value;
                    RaisePropertyChanged("MaximumAvailableBonusPoints");
                }
            }
        }

        private string _description;

        public string Description
        {
            get
            {
                return this._description;
            }

            set
            {
                if (value != _description)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }
        private string _name;
        private int _money;
        private int _experience;

        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
        public override event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;
    }
}
