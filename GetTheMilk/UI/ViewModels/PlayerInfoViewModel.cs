﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly Player _player;

        public PlayerInfoViewModel()
        {
            _player = Player.GetNewInstance();
            PlayerName = _player.Name;
            PlayerHealth = _player.Health;
            PlayerExperience = _player.Experience;
            PlayerMoney = _player.Walet.CurrentCapacity;
            PlayerRightHandObject = (_player.RightHandObject.Objects == null || !_player.RightHandObject.Objects.Any()) ? "" : _player.RightHandObject.Objects[0].Name;
            PlayerLeftHandObject = (_player.LeftHandObject.Objects == null || !_player.LeftHandObject.Objects.Any()) ? "" : _player.LeftHandObject.Objects[0].Name;

        }
    }
}