using System;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
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
                        Player player = Player.GetNewInstance();
                        var actionResult = player.EnterLevel(_level);
                        Story = string.Format("{0}\r\n{1}",_level.Story, RecordActionResult(actionResult, player));
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

        private string RecordActionResult(ActionResult actionResult, Player player)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return string.Format("\r\n{0}\r\n{1}", 
                                 actionResultToHuL.TranslateActionResult(
                                     actionResult, player),
                                 actionResultToHuL.TranslateMovementExtraData(
                                     actionResult.ExtraData as MovementActionExtraData, player, _level));
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
                    if (_actionPanelViewModel != null)
                        _actionPanelViewModel.ActionExecutionRequest -= _actionPanelViewModel_ActionExecutionRequest;
                    _actionPanelViewModel = value;
                    _actionPanelViewModel.ActionExecutionRequest += _actionPanelViewModel_ActionExecutionRequest;
                    RaisePropertyChanged("ActionPanelViewModel");
                }
            }
        }

        void _actionPanelViewModel_ActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
            Story += RecordActionResult(_playerInfoViewModel.PlayerDoesAction(e.GameAction, _level.Maps,
                                                                       _level.PositionableObjects.Objects,
                                                                       _level.Characters.Objects),Player.GetNewInstance());

        }

        public GamePlayViewModel()
        {
            
            PlayerInfoViewModel=new PlayerInfoViewModel();
            ActionPanelViewModel= new ActionPanelViewModel();
          
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
