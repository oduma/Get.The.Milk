using System;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.UI.Translators;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class GamePlayViewModel:GameBaseViewModel
    {
        private int _levelNo;

        public int LevelNo
        {
            get { return _levelNo; }
            set
            {
                if(value!=_levelNo)
                {
                    try
                    {
                        _level = (new LevelsFactory().CreateLevel(value));
                        Story = string.Format("{0}\r\n{1}", _level.Story,
                                              (new ActionResultToHuL()).TranslateMovementResult(
                                                  Player.GetNewInstance().EnterLevel(_level), Player.GetNewInstance()));
                    }
                    catch
                    {
                        Story = "Level " + value + " doesn't exist.";
                    }

                    _levelNo = value;
                    RaisePropertyChanged("LevelNo");
                }
            }
        }

        private ILevel _level;

        private string _story;
        public string Story
        {
            get { return _story; }
            set
            {
                if (value != _story)
                {
                    _story = value;
                    RaisePropertyChanged("Story");
                }
            }
        }

        private PlayerInfoViewModel _playerInfoViewModel;

        public PlayerInfoViewModel PlayerInfoViewModel
        {
            get { return _playerInfoViewModel; }
            set
            {
                if(value != _playerInfoViewModel)
                {
                    _playerInfoViewModel = value;
                    RaisePropertyChanged("PlayerInfoViewModel");
                }
            }
        }

        public GamePlayViewModel()
        {
            
            PlayerInfoViewModel=new PlayerInfoViewModel();
          
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
