using System;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
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
                        Story = string.Format("{0}\r\n{1}",_level.Story, RecordMovementResult(actionResult, player));
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

        private string RecordMovementResult(ActionResult actionResult, Player player)
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
                                 actionResultToHuL.TranslateMovementResult(
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
            if (e.GameAction is MovementAction)
                Story += RecordMovementResult(_playerInfoViewModel.PlayerMoves(e.GameAction, _level.Maps,
                                                                       _level.PositionableObjects.Objects,
                                                                       _level.Characters.Objects), Player.GetNewInstance());
            else
                Story += RecordActionResult(_playerInfoViewModel.PlayerDoesAction(e.GameAction, e.TargetObject),
                                            Player.GetNewInstance(), e.TargetObject);
        }

        private string RecordActionResult(ActionResult actionResult,IPlayer player,IPositionableObject targetObject)
        {
            var actionResultToHuL = new ActionResultToHuL();
            _actionPanelViewModel.DisplayPossibleActions(
                _level.PositionableObjects.Objects.Where(
                    o => o.MapNumber == player.MapNumber && o.CellNumber == player.CellNumber).ToArray());
            _playerInfoViewModel.PlayerLeftHandObject = (player.LeftHandObject.Objects.Any())?player.LeftHandObject.Objects[0].Name.Main:string.Empty;
            _playerInfoViewModel.PlayerRightHandObject = (player.RightHandObject.Objects.Any())?player.RightHandObject.Objects[0].Name.Main:string.Empty;
            return string.Format("\r\n{0}\r\n",
                                 actionResultToHuL.TranslateActionResult(
                                     actionResult,player,targetObject));
            

        }

        public GamePlayViewModel()
        {
            
            PlayerInfoViewModel=new PlayerInfoViewModel();
            ActionPanelViewModel= new ActionPanelViewModel();
          
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
