using System;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.Translators;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System.Windows;

namespace GetTheMilk.UI.ViewModels
{
    public class GamePlayViewModel:GameBaseViewModel
    {
        private Game _game;

        private int _levelNo;

        public int LevelNo
        {
            get { return _levelNo; }
            set
            {
                if(value!=_levelNo)
                {
                    _levelNo = value;
                    RaisePropertyChanged("LevelNo");
                }
            }
        }

        private string RecordMovementResult(ActionResult actionResult)
        {
            var actionResultToHuL = new ActionResultToHuL();
            var additionalInformation = string.Empty;
            if(actionResult.ExtraData is MovementActionExtraData )
            {
                var movementExtraData = actionResult.ExtraData as MovementActionExtraData;
                additionalInformation = actionResultToHuL.TranslateMovementExtraData(movementExtraData, _game.Player, _game.CurrentLevel);
                _actionPanelViewModel.DisplayPossibleActions(movementExtraData.ObjectsInCell.Union(movementExtraData.ObjectsInRange).ToArray());
            }
            _playerInfoViewModel.PlayerCurrentPosition = _game.Player.CellNumber;
            return string.Format("\r\n{0}\r\n{1}", 
                                 actionResultToHuL.TranslateMovementResult(
                                     actionResult, _game.Player),additionalInformation);
            
        }

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

        private Visibility _storyVisible;
        public Visibility StoryVisible
        {
            get { return _storyVisible; }
            set
            {
                if (value != _storyVisible)
                {
                    _storyVisible = value;
                    RaisePropertyChanged("StoryVisible");
                }
            }
        }

        private Visibility _inventoryVisible;
        public Visibility InventoryVisible
        {
            get { return _inventoryVisible; }
            set
            {
                if (value != _inventoryVisible)
                {
                    _inventoryVisible = value;
                    RaisePropertyChanged("InventoryVisible");
                }
            }
        }

        private InventoryViewModel _inventoryViewModel;

        public InventoryViewModel InventoryViewModel
        {
            get { return _inventoryViewModel; }
            set
            {
                if (value != _inventoryViewModel)
                {
                    _inventoryViewModel = value;
                    RaisePropertyChanged("InventoryViewModel");
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
            if (e.GameAction is MovementAction)
                Story += RecordMovementResult(_playerInfoViewModel.PlayerMoves(e.GameAction as MovementAction, _game.CurrentLevel.CurrentMap));
            else if(e.GameAction==null)
            {
                StoryVisible = Visibility.Visible;
                InventoryVisible = Visibility.Hidden;
            }
            else if (e.GameAction is ExposeInventory)
            {

                var actionResult = e.GameAction.Perform();
                if (actionResult.ResultType == ActionResultType.Ok)
                {
                    StoryVisible = Visibility.Hidden;
                    InventoryVisible = Visibility.Visible;
                    InventoryViewModel = new InventoryViewModel(_game.Player.Name.Main,actionResult.ExtraData as ExposeInventoryExtraData);

                }
            }
            else

                Story += RecordActionResult(e.GameAction.Perform(),
                                             e.GameAction.TargetObject);
        }

        private string RecordActionResult(ActionResult actionResult, NonCharacterObject targetObject)
        {
            var actionResultToHuL = new ActionResultToHuL();
            _actionPanelViewModel.DisplayPossibleActions(
                _game.CurrentLevel.CurrentMap.Cells[_game.Player.CellNumber].AllObjects);
            return string.Format("\r\n{0}\r\n",
                                 actionResultToHuL.TranslateActionResult(
                                     actionResult, _game.Player, targetObject));


        }

        public GamePlayViewModel(Game game)
        {
            _game=game;
            var actionResult = _game.Player.EnterLevel(_game.CurrentLevel);
            LevelNo = _game.CurrentLevel.Number;
            PlayerInfoViewModel=new PlayerInfoViewModel(_game);
            ActionPanelViewModel= new ActionPanelViewModel(_game.Player);
            Story = string.Format("{0}\r\n{1}", _game.CurrentLevel.Story, RecordMovementResult(actionResult));
            StoryVisible = Visibility.Visible;
            InventoryVisible = Visibility.Hidden;
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;

    }
}
