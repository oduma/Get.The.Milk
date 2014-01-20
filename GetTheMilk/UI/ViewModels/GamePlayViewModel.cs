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
            var actionResultToHuL = new ActionResultToHuL();
            var additionalInformation = string.Empty;
            if(actionResult.ExtraData is MovementActionExtraData )
            {
                var movementExtraData = actionResult.ExtraData as MovementActionExtraData;
                additionalInformation = actionResultToHuL.TranslateMovementExtraData(movementExtraData, player, _level);
                _actionPanelViewModel.DisplayPossibleActions(movementExtraData.ObjectsInCell);
            }
            _playerInfoViewModel.PlayerCurrentPosition = player.CellNumber;
            return string.Format("\r\n{0}\r\n{1}", 
                                 actionResultToHuL.TranslateActionResult(
                                     actionResult, player),additionalInformation);
            
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
                        _actionPanelViewModel.ActionExecutionRequest -= ActionPanelViewModelActionExecutionRequest;
                    _actionPanelViewModel = value;
                    _actionPanelViewModel.ActionExecutionRequest += ActionPanelViewModelActionExecutionRequest;
                    RaisePropertyChanged("ActionPanelViewModel");
                }
            }
        }

        void ActionPanelViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
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
