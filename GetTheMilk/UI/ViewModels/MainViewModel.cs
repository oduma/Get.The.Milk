using System;
using GetTheMilk.Settings;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
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
                    }
                    _currentGameViewModel = value;
                    _currentGameViewModel.GameStartRequest += CurrentGameViewModelGameStartRequest;
                    RaisePropertyChanged("CurrentGameViewModel");
                }
            }

        }

        void CurrentGameViewModelGameStartRequest(object source, GameStartRequestEventArgs eventArgs)
        {
            CurrentGameViewModel = new GamePlayViewModel(eventArgs.Game);
        }

        public MainViewModel()
        {
            Title = GameSettings.GetInstance().DefaultGameName;
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
            var game=Game.CreateGameInstance();
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
        public string Title { get; set; }
    }
}
