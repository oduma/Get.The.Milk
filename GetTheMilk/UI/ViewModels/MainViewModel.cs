using GetTheMilk.Settings;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        public RelayCommand ExitCommand { get; private set; }

        public RelayCommand NewCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }
        
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
        }

        private void SaveGame()
        {
            var fileName = string.Empty;
            _game.Save(fileName);
        }

        private void LoadPlayerSetup()
        {
            CurrentGameViewModel=new PlayerSetupViewModel();
        }


        private void Exit()
        {
            ;
        }

        private readonly Game _game;

        public string Title { get; set; }
    }
}
