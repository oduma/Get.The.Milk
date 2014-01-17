using System;
using GetTheMilk.Actions;
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
                        ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
                        Player player = Player.GetNewInstance();
                        var actionResult = player.EnterLevel(_level);
                        Story = string.Format("{0}\r\n{1}\r\n{2}", _level.Story,
                                              actionResultToHuL.TranslateActionResult(
                                                  actionResult, player),
                                              actionResultToHuL.TranslateMovementExtraData(
                                                  actionResult.ExtraData as MovementActionExtraData, player, _level));
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

        private ActionPanelViewModel _actionPanelViewModel;

        public ActionPanelViewModel ActionPanelViewModel
        {
            get { return _actionPanelViewModel; }
            set
            {
                if (value != _actionPanelViewModel)
                {
                    _actionPanelViewModel = value;
                    RaisePropertyChanged("ActionPanelViewModel");
                }
            }
        }

        public GamePlayViewModel()
        {
            
            PlayerInfoViewModel=new PlayerInfoViewModel();
            ActionPanelViewModel= new ActionPanelViewModel();
          
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
