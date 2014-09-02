using System;
using System.Collections.ObjectModel;
using System.IO;
using Get.The.Milk.UI.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class LoadLevelViewModel:ViewModelBase
    {
        private readonly string _fromFolder;
        private readonly string _withPattern;
        private readonly Action<string> _loadMethod;
        public RelayCommand Load { get; private set; }

        public LoadLevelViewModel(string fromFolder,string withPattern,Action<string> loadMethod)
        {
            _fromFolder = fromFolder;
            _withPattern = withPattern;
            _loadMethod = loadMethod;
            Load = new RelayCommand(LoadLevel);
            Message = "Pick a file";
        }

        private void LoadLevel()
        {
            _loadMethod(SelectedFileName);
        }

        private ObservableCollection<string> _allFiles;

        public ObservableCollection<string> AllFiles
        {
            get
            {
                _allFiles=new ObservableCollection<string>();
                foreach (
                    var file in Directory.GetFiles(_fromFolder, _withPattern))
                {
                    _allFiles.Add(file);
                }
                return _allFiles;
            }
        }

        private string _selectedFileName;
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

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    RaisePropertyChanged("Message");
                }
            }
        }

    }
}
