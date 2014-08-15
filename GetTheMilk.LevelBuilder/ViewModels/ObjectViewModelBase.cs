using System.Collections.ObjectModel;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.BaseCommon;

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

        private InteractionsViewModel _currentInteractionsViewModel;
        public InteractionsViewModel CurrentInteractionsViewModel
        {
            get { return _currentInteractionsViewModel; }
            set
            {
                if (value != _currentInteractionsViewModel)
                {
                    _currentInteractionsViewModel = value;
                    RaisePropertyChanged("CurrentInteractionsiewModel");
                }
            }
        }

        public abstract ObjectViewModelBase<T> Clone();
    }
}
