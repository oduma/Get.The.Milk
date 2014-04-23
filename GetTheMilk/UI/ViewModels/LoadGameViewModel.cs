using System;
using System.Collections.ObjectModel;
using System.IO;
using GetTheMilk.Settings;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class LoadGameViewModel:GameBaseViewModel
    {
        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
        public override event EventHandler<GameAdvanceRequestEventArgs> GameAdvanceRequest;
        public RelayCommand Load { get; private set; }

        public LoadGameViewModel()
        {
            Load = new RelayCommand(LoadGame);

        }

        private void LoadGame()
        {
            var game = Game.Load(SelectedFileName);
            if (GameStartRequest != null)
            {
                GameStartRequest(this, new GameStartRequestEventArgs(game));
            }
        }

        private ObservableCollection<string> _savedGames;
        private string _selectedFileName;

        public ObservableCollection<string> SavedGames
        {
            get
            {
                _savedGames=new ObservableCollection<string>();
                foreach (
                    var file in Directory.GetFiles(GameSettings.GetInstance().DefaultPaths.SaveDefaultPath, "*.gsu"))
                {
                    _savedGames.Add(file);
                }
                return _savedGames;
            }
        }

        public string SelectedFileName
        {
            get { return _selectedFileName; }
            set
            {
                if (value != _selectedFileName)
                {
                    _selectedFileName = value;
                    RaisePropertyChanged("SelectedFileName");
                }
            }
        }

    }
}
