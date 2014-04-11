using System;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
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
                _actionPanelViewModel.DisplayPossibleActions(movementExtraData.AvailableActions);
            }
            _playerInfoViewModel.PlayerCurrentPosition = _game.Player.CellNumber;
            return string.Format("\r\n{0}\r\n{1}", 
                                 actionResultToHuL.TranslateMovementResult(
                                     actionResult),additionalInformation);
            
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

        private Visibility _twoCharactersVisible;
        public Visibility TwoCharactersVisible
        {
            get { return _twoCharactersVisible; }
            set
            {
                if (value != _twoCharactersVisible)
                {
                    _twoCharactersVisible = value;
                    RaisePropertyChanged("TwoCharactersVisible");
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
                    if(_inventoryViewModel!=null)
                        _inventoryViewModel.ActionExecutionRequest -= InventoryViewModelActionExecutionRequest;
                    _inventoryViewModel = value;
                    _inventoryViewModel.ActionExecutionRequest += InventoryViewModelActionExecutionRequest;
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

        private TwoCharactersViewModel _twoCharactersViewModel;

        public TwoCharactersViewModel TwoCharactersViewModel
        {
            get { return _twoCharactersViewModel; }
            set
            {
                if (value != _twoCharactersViewModel)
                {
                    if (_twoCharactersViewModel != null)
                        _twoCharactersViewModel.ActionExecutionRequest -= TwoCharactersViewModelActionExecutionRequest;
                    _twoCharactersViewModel = value;
                    _twoCharactersViewModel.ActionExecutionRequest += TwoCharactersViewModelActionExecutionRequest;
                    RaisePropertyChanged("TwoCharactersViewModel");
                }
            }
        }

        void TwoCharactersViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
            if (e.GameAction.FinishTheInteractionOnExecution)
            {
                StoryVisible = Visibility.Visible;
                InventoryVisible = Visibility.Hidden;
                TwoCharactersVisible = Visibility.Hidden;
            }
            ActionPanelViewModelActionExecutionRequest(sender, e);
        }

        void InventoryViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
                if (e.GameAction.FinishTheInteractionOnExecution)
                {
                    StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    ActionPanelViewModelActionExecutionRequest(sender,e);
                    ActionPanelViewModel.InventoryShowHide = "Show Inventory";
                }
                else
                {
                    ActionPanelViewModelActionExecutionRequest(sender, e);
                }
        }

        void ActionPanelViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
            if (e.GameAction is MovementAction)
            {
                ((MovementAction) e.GameAction).CurrentMap = _game.CurrentLevel.CurrentMap;
                Story += RecordMovementResult(e.GameAction.Perform());
            }
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
                    TwoCharactersVisible=Visibility.Hidden;
                    InventoryViewModel = new InventoryViewModel(actionResult.ForAction.ActiveCharacter.Name.Main,actionResult.ExtraData as ExposeInventoryExtraData);

                }
            }
            else if (e.GameAction is TwoCharactersAction)
            {
                if (e.GameAction.FinishTheInteractionOnExecution)
                {
                    StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Hidden;
                    Story += RecordActionResult(e.GameAction.Perform());
                }
                else
                {
                    StoryVisible = Visibility.Hidden;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Visible;
                    TwoCharactersViewModel = new TwoCharactersViewModel(e.GameAction);

                }
            }
            else

                Story += RecordActionResult(e.GameAction.Perform());
        }

        private string RecordActionResult(ActionResult actionResult)
        {
            if (actionResult.ForAction.ActionType == ActionType.ExposeInventory)
            {
                ActionPanelViewModelActionExecutionRequest(this,new ActionExecutionRequestEventArgs(actionResult.ForAction));
            }
            if (actionResult.ForAction.ActionType == ActionType.Buy)
            {
                _playerInfoViewModel.PlayerMoney = _game.Player.Walet.CurrentCapacity;
                var objectBought =
                    _inventoryViewModel.Weapons.First(
                        o => o.ObjectName == actionResult.ForAction.TargetObject.Name.Main);
                if (actionResult.ForAction.TargetObject.ObjectCategory == ObjectCategory.Weapon)
                {
                    _inventoryViewModel.Weapons.Remove(objectBought);
                }
                else if (actionResult.ForAction.TargetObject.ObjectCategory == ObjectCategory.Tool)
                {
                    _inventoryViewModel.Tools.Remove(objectBought);
                }
            }
            var actionResultToHuL = new ActionResultToHuL();
            var teleport = new Teleport();
            teleport.ActiveCharacter = _game.Player;
            teleport.CurrentMap = _game.CurrentLevel.CurrentMap;
            teleport.TargetCell = _game.Player.CellNumber;

            _actionPanelViewModel.DisplayPossibleActions(
                ((MovementActionExtraData)teleport.Perform().ExtraData).AvailableActions);
            return string.Format("\r\n{0}\r\n",
                                 actionResultToHuL.TranslateActionResult(
                                     actionResult));


        }

        public GamePlayViewModel(Game game)
        {
            _game=game;
            ActionPanelViewModel = new ActionPanelViewModel(_game.Player);
            PlayerInfoViewModel = new PlayerInfoViewModel(_game);
            var actionResult = _game.Player.EnterLevel(_game.CurrentLevel);
            LevelNo = _game.CurrentLevel.Number;
            Story = string.Format("{0}\r\n{1}", _game.CurrentLevel.Story, RecordMovementResult(actionResult));
            StoryVisible = Visibility.Visible;
            InventoryVisible = Visibility.Hidden;
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;

    }
}
