using System;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Settings;
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
            if(actionResult.ResultType==ActionResultType.LevelCompleted)
            {
                var levelFinishedMessage = _game.CurrentLevel.FinishMessage;
                if(GameAdvanceRequest!=null)
                {
                    if (_game.ProceedToNextLevel())
                        GameAdvanceRequest(this,new GameAdvanceRequestEventArgs(_game,levelFinishedMessage,"Go to Next Level"));
                    else
                    {
                        GameAdvanceRequest(this,new GameAdvanceRequestEventArgs(_game,levelFinishedMessage + "\r\n" + GameSettings.GetInstance().GameFinishingMessage,string.Empty));
                    }
                }
                return string.Empty;
            }
            else
            {
                var actionResultToHuL = new ActionResultToHuL();
                var additionalInformation = string.Empty;
                if (actionResult.ExtraData is MovementActionExtraData)
                {
                    var movementExtraData = actionResult.ExtraData as MovementActionExtraData;
                    additionalInformation = actionResultToHuL.TranslateMovementExtraData(movementExtraData, _game.Player, _game.CurrentLevel);
                    ActionPanelViewModel.DisplayPossibleActions(movementExtraData.AvailableActions);
                }
                PlayerInfoViewModel.PlayerCurrentPosition = _game.Player.CellNumber;
                return string.Format("\r\n{0}\r\n{1}",
                                     actionResultToHuL.TranslateMovementResult(
                                         actionResult), additionalInformation);
            }
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
                    _actionPanelViewModel = value;
                    if (_actionPanelViewModel != null)
                    {
                        _actionPanelViewModel.ActionExecutionRequest -= ActionPanelViewModelActionExecutionRequest;
                        _actionPanelViewModel.ActionExecutionRequest += ActionPanelViewModelActionExecutionRequest;
                    }
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
                    {
                        _twoCharactersViewModel.ActionExecutionRequest -= TwoCharactersViewModelActionExecutionRequest;
                        _twoCharactersViewModel.PlayerStatsUpdateRequest -= TwoCharactersViewModelPlayerStatsUpdateRequest;
                        _twoCharactersViewModel.FeedbackFromSubAction -= GamePlayViewModelFeedbackFromSubAction;
                    }
                    _twoCharactersViewModel = value;
                    _twoCharactersViewModel.ActionExecutionRequest += TwoCharactersViewModelActionExecutionRequest;
                    _twoCharactersViewModel.PlayerStatsUpdateRequest +=TwoCharactersViewModelPlayerStatsUpdateRequest;
                    _twoCharactersViewModel.FeedbackFromSubAction += GamePlayViewModelFeedbackFromSubAction;
                    RaisePropertyChanged("TwoCharactersViewModel");
                }
            }
        }

        private void TwoCharactersViewModelPlayerStatsUpdateRequest(object sender, PlayerStatsUpdateRequestEventArgs e)
        {
            var playerCharacter = (e.ActionResult.ForAction.ActiveCharacter is IPlayer)
                                      ? e.ActionResult.ForAction.ActiveCharacter
                                      : e.ActionResult.ForAction.TargetCharacter;
            PlayerInfoViewModel.PlayerHealth = playerCharacter.Health;
            PlayerInfoViewModel.PlayerExperience = playerCharacter.Experience;
            PlayerInfoViewModel.PlayerMoney = playerCharacter.Walet.CurrentCapacity;
            if(e.ActionResult.ForAction.ActionType==ActionType.AcceptQuit 
                || e.ActionResult.ResultType==ActionResultType.Win)
            {
                StoryVisible = Visibility.Visible;
                TwoCharactersVisible = Visibility.Hidden;
                Story += RecordActionResult(e.ActionResult);
            }
            else if(e.ActionResult.ResultType==ActionResultType.Lost)
            {
                if(GameAdvanceRequest!=null)
                    GameAdvanceRequest(this, new GameAdvanceRequestEventArgs(_game, (new ActionResultToHuL()).TranslateActionResult(e.ActionResult),string.Empty));
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
                    if (e.GameAction is TwoCharactersAction)
                        StoryVisible = Visibility.Hidden;
                    else
                        StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    ActionPanelViewModel.InventoryShowHide = "Show Inventory";
                }
                ActionPanelViewModelActionExecutionRequest(sender, e);
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
                if (e.GameAction.FinishTheInteractionOnExecution 
                    && (e.GameAction.ActionType==ActionType.Communicate 
                    || e.GameAction.ActionType==ActionType.AcceptQuit)) //the last words
                {
                    StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Hidden;
                    ((TwoCharactersAction)e.GameAction).FeedbackFromSubAction -= GamePlayViewModelFeedbackFromSubAction;
                    ((TwoCharactersAction)e.GameAction).FeedbackFromSubAction += GamePlayViewModelFeedbackFromSubAction;
                    var actionResult = e.GameAction.Perform();
                    Story += RecordActionResult(actionResult);
                }
                else
                {
                    StoryVisible = Visibility.Hidden;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Visible;
                    TwoCharactersViewModel = new TwoCharactersViewModel(e.GameAction.ActiveCharacter,e.GameAction.TargetCharacter);
                    TwoCharactersViewModel.ExecuteAction(e.GameAction as TwoCharactersAction);

                }
            }
            else

                Story += RecordActionResult(e.GameAction.Perform());
        }

        void GamePlayViewModelFeedbackFromSubAction(object sender, FeedbackEventArgs e)
        {
            Story+=RecordActionResult(e.ActionResult);
        }

        private string RecordActionResult(ActionResult actionResult)
        {
            if (actionResult.ForAction.ActionType == ActionType.ExposeInventory)
            {
                ActionPanelViewModelActionExecutionRequest(this,new ActionExecutionRequestEventArgs(actionResult.ForAction));
            }
            if (actionResult.ForAction is ObjectTransferFromAction 
                || actionResult.ForAction.ActionType==ActionType.TakeMoneyFrom)
            {
                PlayerInfoViewModel.PlayerMoney = _game.Player.Walet.CurrentCapacity;
                if(InventoryVisible==Visibility.Visible)
                {
                    InventoryViewModel.Remove(actionResult.ForAction.TargetObject);
                }
            }
            var actionResultToHuL = new ActionResultToHuL();
            var teleport = new Teleport();
            teleport.ActiveCharacter = _game.Player;
            teleport.CurrentMap = _game.CurrentLevel.CurrentMap;
            teleport.TargetCell = _game.Player.CellNumber;

            ActionPanelViewModel.DisplayPossibleActions(
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
        public override event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;
    }
}
