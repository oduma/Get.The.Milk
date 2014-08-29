using System;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Settings;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System.Windows;

namespace GetTheMilk.UI.ViewModels
{
    public class GamePlayViewModel:GameBaseViewModel
    {
        private RpgGameCore _game;

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

        private string RecordMovementResult(PerformActionResult actionResult)
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
                var additionalInformation = string.Empty;
                if (actionResult.ExtraData is MovementActionTemplateExtraData)
                {
                    var movementExtraData = actionResult.ExtraData as MovementActionTemplateExtraData;
                    additionalInformation =movementExtraData.ToString();
                    ActionPanelViewModel.DisplayPossibleActions(movementExtraData.AvailableActionTemplates);
                }
                PlayerInfoViewModel.PlayerCurrentPosition = _game.Player.CellNumber;
                return string.Format("\r\n{0}\r\n{1}",
                                     actionResult.ToString(), additionalInformation);
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
            //if(e.ActionResult.ForAction.Name.UniqueId=="AcceptQuit" 
            //    || e.ActionResult.ResultType==ActionResultType.Win)
            //{
            //    StoryVisible = Visibility.Visible;
            //    TwoCharactersVisible = Visibility.Hidden;
            //    Story += RecordActionResult(e.ActionResult);
            //}
            //else if(e.ActionResult.ResultType==ActionResultType.Lost)
            //{
            //    if(GameAdvanceRequest!=null)
            //        GameAdvanceRequest(this, new GameAdvanceRequestEventArgs(_game, e.ActionResult.ToString(),string.Empty));
            //}
        }

        void TwoCharactersViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
                StoryVisible = Visibility.Visible;
                InventoryVisible = Visibility.Hidden;
                TwoCharactersVisible = Visibility.Hidden;
            ActionPanelViewModelActionExecutionRequest(sender, e);
        }

        void InventoryViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
                    if (e.GameAction.GetType() == typeof(TwoCharactersActionTemplate))
                        StoryVisible = Visibility.Hidden;
                    else
                        StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    ActionPanelViewModel.InventoryShowHide = "Show Inventory";
                ActionPanelViewModelActionExecutionRequest(sender, e);
        }

        void ActionPanelViewModelActionExecutionRequest(object sender, ActionExecutionRequestEventArgs e)
        {
            if (e.GameAction is MovementActionTemplate)
            {
                ((MovementActionTemplate) e.GameAction).CurrentMap = _game.CurrentLevel.CurrentMap;
                Story += RecordMovementResult(_game.Player.PerformAction(e.GameAction));
            }
            else if(e.GameAction==null)
            {
                StoryVisible = Visibility.Visible;
                InventoryVisible = Visibility.Hidden;
            }
            else if (e.GameAction is ExposeInventoryActionTemplate)
            {

                var actionResult = e.GameAction.ActiveCharacter.PerformAction(e.GameAction);
                if (actionResult.ResultType == ActionResultType.Ok)
                {
                    StoryVisible = Visibility.Hidden;
                    InventoryVisible = Visibility.Visible;
                    TwoCharactersVisible=Visibility.Hidden;
                    InventoryViewModel = new InventoryViewModel(actionResult.ForAction.ActiveCharacter.Name.Main,actionResult.ExtraData as InventoryExtraData);

                }
            }
            else if (e.GameAction is TwoCharactersActionTemplate)
            {
                if ((e.GameAction.CurrentPerformer.GetType() == typeof(CommunicateActionPerformer) 
                    || e.GameAction.Name.UniqueId=="AcceptQuit")) //the last words
                {
                    StoryVisible = Visibility.Visible;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Hidden;
                    ((TwoCharactersActionTemplatePerformer)e.GameAction.CurrentPerformer).FeedbackFromSubAction -= GamePlayViewModelFeedbackFromSubAction;
                    ((TwoCharactersActionTemplatePerformer)e.GameAction.CurrentPerformer).FeedbackFromSubAction += GamePlayViewModelFeedbackFromSubAction;
                    var actionResult = e.GameAction.Perform();
                    Story += RecordActionResult(actionResult);
                }
                else
                {
                    StoryVisible = Visibility.Hidden;
                    InventoryVisible = Visibility.Hidden;
                    TwoCharactersVisible = Visibility.Visible;
                    TwoCharactersViewModel = new TwoCharactersViewModel(e.GameAction.ActiveCharacter,e.GameAction.TargetCharacter);
                    
                    TwoCharactersViewModel.ExecuteAction((TwoCharactersActionTemplate)e.GameAction);

                }
            }
            else
            {
                Story += RecordActionResult(e.GameAction.ActiveCharacter.PerformAction(e.GameAction));
            }
        }

        void GamePlayViewModelFeedbackFromSubAction(object sender, FeedbackEventArgs e)
        {
            Story+=RecordActionResult(e.ActionResult);
        }

        private string RecordActionResult(PerformActionResult actionResult)
        {
            if (actionResult.ForAction.Name.UniqueId == "ExposeInventory")
            {
                ActionPanelViewModelActionExecutionRequest(this,new ActionExecutionRequestEventArgs(actionResult.ForAction));
            }
            if (actionResult.ForAction.GetType() == typeof(ObjectTransferActionTemplate))
            {
                PlayerInfoViewModel.PlayerMoney = _game.Player.Walet.CurrentCapacity;
                if(InventoryVisible==Visibility.Visible)
                {
                    InventoryViewModel.Remove(actionResult.ForAction.TargetObject);
                }
            }
            var teleport =
                _game.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");

            teleport.ActiveCharacter = _game.Player;
            teleport.CurrentMap = _game.CurrentLevel.CurrentMap;
            teleport.TargetCell = _game.Player.CellNumber;

            
            ActionPanelViewModel.DisplayPossibleActions(
                ((MovementActionTemplateExtraData)_game.Player.PerformAction(teleport).ExtraData).AvailableActionTemplates);
            return string.Format("\r\n{0}\r\n",
                                 actionResult.ToString());


        }

        public GamePlayViewModel(RpgGameCore game)
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
