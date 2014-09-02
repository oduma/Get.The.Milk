using System;
using System.Windows;
using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        public RelayCommand ExitCommand { get; private set; }

        public RelayCommand NewCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand LoadCommand { get; private set; }

        private GameBaseViewModel _currentGameViewModel;

        public GameBaseViewModel CurrentGameViewModel
        {
            get { return _currentGameViewModel; }
            set
            {
                if (value != _currentGameViewModel)
                {
                    if (_currentGameViewModel != null)
                    {
                        _currentGameViewModel.GameStartRequest -= CurrentGameViewModelGameStartRequest;
                        _currentGameViewModel.GameAdvanceRequest -= _currentGameViewModel_GameAdvanceRequest;

                    }
                    _currentGameViewModel = value;
                    _currentGameViewModel.GameStartRequest += CurrentGameViewModelGameStartRequest;
                    _currentGameViewModel.GameAdvanceRequest += _currentGameViewModel_GameAdvanceRequest;
                    RaisePropertyChanged("CurrentGameViewModel");
                }
            }

        }

        void _currentGameViewModel_GameAdvanceRequest(object sender, GameAdvanceRequestEventArgs e)
        {
            CurrentGameViewModel = new GameAdvanceViewModel
            {
                ActionName = e.ActionName,
                ActionVisible = (string.IsNullOrEmpty(e.ActionName))?Visibility.Hidden:Visibility.Visible,
                Message = e.Message,
                Game=e.Game
            };
            CurrentGameViewModel.GameStartRequest -= CurrentGameViewModelGameStartRequest;
            CurrentGameViewModel.GameStartRequest += CurrentGameViewModelGameStartRequest;
        }

        void CurrentGameViewModelGameStartRequest(object source, GameStartRequestEventArgs eventArgs)
        {
            CurrentGameViewModel = new GamePlayViewModel(eventArgs.Game);
        }

        public MainViewModel()
        {
            ExitCommand = new RelayCommand(Exit);
            CurrentGameViewModel=new GameViewModel();
            NewCommand=new RelayCommand(LoadPlayerSetup);
            SaveCommand=new RelayCommand(SaveGame);
            LoadCommand=new RelayCommand(LoadGame);
        }

        private void LoadGame()
        {
            CurrentGameViewModel=new LoadGameViewModel();
        }

        private void SaveGame()
        {
            var fileName = string.Format("{0}.{1}",Guid.NewGuid().ToString(),"gsu");
            var game=RpgGameCore.GetGameInstance();
            game.Save(fileName);
        }

        private void LoadPlayerSetup()
        {
            CurrentGameViewModel=new PlayerSetupViewModel();
        }


        private void Exit()
        {
            ;
        }
    }
}
