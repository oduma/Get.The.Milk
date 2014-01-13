using System;
using GetTheMilk.Factories;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private Game _game;

        public RelayCommand ExitCommand { get; private set; }

        public RelayCommand NewCommand { get; private set; }
        
        private GameBaseViewModel _currentGameViewModel;

        public GameBaseViewModel CurrentGameViewModel
        {
            get { return _currentGameViewModel; }
            set
            {
                if (value != _currentGameViewModel)
                {
                    if (_currentGameViewModel != null)
                        _currentGameViewModel.GameStartRequest -= _currentGameViewModel_GameStartRequest;
                    _currentGameViewModel = value;
                    _currentGameViewModel.GameStartRequest += _currentGameViewModel_GameStartRequest;
                    RaisePropertyChanged("CurrentGameViewModel");
                }
            }

        }

        void _currentGameViewModel_GameStartRequest(object source, GameStartRequestEventArgs eventArgs)
        {
            try
            {
                //CurrentGameViewModel = (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>("TextBased") as
                //    GamePlayViewModel;
                CurrentGameViewModel = new GamePlayViewModel();

                ((GamePlayViewModel)CurrentGameViewModel).LevelNo = eventArgs.Level;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public MainViewModel()
        {
            _game = Game.CreateGameInstance();
            Title = _game.Name;
            ExitCommand = new RelayCommand(Exit);
            CurrentGameViewModel=new GameViewModel();
            NewCommand=new RelayCommand(LoadPlayerSetup);
        }

        private void LoadPlayerSetup()
        {
            CurrentGameViewModel=new PlayerSetupViewModel();
        }


        private void Exit()
        {
            ;
        }

        private string _title;

        public string Title
        {
            get { return _title; }

            set { _title = value; }
        }


    }
}
