using System.Collections.ObjectModel;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public abstract class ObjectViewModelBase:ViewModelBase
    {
        public abstract NonCharacterObject Value { get; set; }

        private ObservableCollection<string> _allObjectTypes;
        public ObservableCollection<string> AllObjectTypes
        {
            get { return _allObjectTypes; }
            set
            {
                if (value != _allObjectTypes)
                {
                    _allObjectTypes = value;
                    RaisePropertyChanged("AllObjectTypes");
                }
            }
        }


    }
}
