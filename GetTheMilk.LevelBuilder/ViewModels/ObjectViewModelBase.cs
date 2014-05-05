using System.Collections.ObjectModel;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public abstract class ObjectViewModelBase<T>:ViewModelBase
    {
        public abstract T Value { get; set; }

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
