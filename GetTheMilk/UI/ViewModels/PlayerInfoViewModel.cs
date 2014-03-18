using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Navigation;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerInfoViewModel:ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        public RelayCommand ShowInventory { get; private set; }
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

        private string _playerRightHandObject;

        public string PlayerRightHandObject
        {
            get { return _playerRightHandObject; }
            set
            {
                if (value != _playerRightHandObject)
                {
                    _playerRightHandObject = value;
                    RaisePropertyChanged("PlayerRightHandObject");
                }
            }
        }

        private string _playerLeftHandObject;

        public string PlayerLeftHandObject
        {
            get { return _playerLeftHandObject; }
            set
            {
                if (value != _playerLeftHandObject)
                {
                    _playerLeftHandObject = value;
                    RaisePropertyChanged("PlayerLeftHandObject");
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
            ShowInventory = new RelayCommand(ShowInventoryCommand);
        }

        private void ShowInventoryCommand()
        {
            ExposeInventory exposeInventory = new ExposeInventory();
            exposeInventory.IncludeWallet = false;
            
            if(ActionExecutionRequest!=null)
                ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(exposeInventory,null,_player,_player));
        }

        public ActionResult PlayerMoves(GameAction gameAction, Map[] levelMaps, 
            IEnumerable<NonCharacterObject> allLevelObjects, 
            IEnumerable<Character> allLevelCharacters)
        {
            if (gameAction is MovementAction)
                return _player.TryPerformMove(gameAction as MovementAction, levelMaps.FirstOrDefault(m=>m.Number==_player.MapNumber), allLevelObjects, allLevelCharacters);
            return null;
        }

        public ActionResult PlayerDoesAction(GameAction action, NonCharacterObject targetObject)
        {
            return _player.TryPerformAction(action, targetObject);
        }
    }
}
