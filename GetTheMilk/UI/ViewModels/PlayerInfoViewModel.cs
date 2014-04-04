using GetTheMilk.Characters;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerInfoViewModel:ViewModelBase
    {
        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                if (value != _playerName)
                {
                    _playerName = value;
                    RaisePropertyChanged("PlayerName");
                }
            }
        }

        private int _playerHealth;

        public int PlayerHealth
        {
            get { return _playerHealth; }
            set
            {
                if (value != _playerHealth)
                {
                    _playerHealth = value;
                    RaisePropertyChanged("PlayerHealth");
                }
            }
        }

        private int _playerExperience;

        public int PlayerExperience
        {
            get { return _playerExperience; }
            set
            {
                if (value != _playerExperience)
                {
                    _playerExperience = value;
                    RaisePropertyChanged("PlayerExperience");
                }
            }
        }

        private int _playerMoney;

        public int PlayerMoney
        {
            get { return _playerMoney; }
            set
            {
                if (value != _playerMoney)
                {
                    _playerMoney = value;
                    RaisePropertyChanged("PlayerMoney");
                }
            }
        }

        private int _playerCurrentPosition;

        public int PlayerCurrentPosition
        {
            get { return _playerCurrentPosition; }
            set
            {
                if (value != _playerCurrentPosition)
                {
                    _playerCurrentPosition = value;
                    RaisePropertyChanged("PlayerCurrentPosition");
                }
            }
        }
        private readonly Player _player;

        public PlayerInfoViewModel(Game game)
        {
            _player = game.Player;
            PlayerName = _player.Name.Main;
            PlayerHealth = _player.Health;
            PlayerExperience = _player.Experience;
            PlayerMoney = _player.Walet.CurrentCapacity;
        }
    }
}
