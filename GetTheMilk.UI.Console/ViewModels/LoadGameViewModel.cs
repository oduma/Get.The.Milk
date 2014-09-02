using System.Collections.ObjectModel;
using System.IO;
using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class LoadGameViewModel:GameBaseViewModel
    {
        public RelayCommand Load { get; private set; }

        public LoadGameViewModel()
        {
            Load = new RelayCommand(LoadGame);

        }

        private void LoadGame()
        {
            var game = RpgGameCore.Load(SelectedFileName);
            FireStartRequestEvent(this, new GameStartRequestEventArgs(game));
        }

        private ObservableCollection<string> _savedGames;
        private string _selectedFileName;

        public ObservableCollection<string> AllFiles
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
