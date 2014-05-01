using System.Collections.ObjectModel;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ObjectManagerViewModel:ViewModelBase
    {
        private ObjectViewModelBase _currentObjectViewModel;

        public ObjectViewModelBase CurrentObjectViewModel
        {
            get { return _currentObjectViewModel; }
            set
            {
                if (value != _currentObjectViewModel)
                {
                    _currentObjectViewModel = value;
                    RaisePropertyChanged("CurrentObjectViewModel");
                }
            }

        }


        public RelayCommand<ObjectCategory> CreateNewObject { get; private set; }
        public RelayCommand Done { get; private set; }

        public ObjectManagerViewModel()
        {
            ObjectCategories = 
                new ObservableCollection<ObjectCategory> { ObjectCategory.Decor, ObjectCategory.Tool, ObjectCategory.Weapon };
            CreateNewObject= new RelayCommand<ObjectCategory>(DisplayNewObjectEditor);
            Done= new RelayCommand(DoneCommand);
        }

        private void DispalyObjectEditor()
        {
            switch(SelectedObject.ObjectCategory)
            {
                case ObjectCategory.Decor:

                    CurrentObjectViewModel = new DecorObjectViewModel(SelectedObject);
                    break;
                case ObjectCategory.Tool:
                    CurrentObjectViewModel = new ToolObjectViewModel();
                    break;
                case ObjectCategory.Weapon:
                    CurrentObjectViewModel = new WeaponObjectViewModel();
                    break;
            }

        }

        private void DoneCommand()
        {
            if(AllExistingObjects==null)
                AllExistingObjects=new ObservableCollection<NonCharacterObject>();
            AllExistingObjects.Add(CurrentObjectViewModel.Value);
        }

        private void DisplayNewObjectEditor(ObjectCategory obj)
        {
            switch(obj)
            {
                case ObjectCategory.Decor:
                    
                    CurrentObjectViewModel = new DecorObjectViewModel(new NonCharacterObject());
                    break;
                    case ObjectCategory.Tool:
                    CurrentObjectViewModel = new ToolObjectViewModel();
                    break;
                    case ObjectCategory.Weapon:
                    CurrentObjectViewModel = new WeaponObjectViewModel();
                    break;
            }
        }

        private NonCharacterObject _selectedObject;
        public NonCharacterObject SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (value != _selectedObject)
                {
                    _selectedObject = value;
                    RaisePropertyChanged("SelectedObject");
                    DispalyObjectEditor();
                }
            }
        }
        private ObservableCollection<NonCharacterObject> _allExistingObjects;
        public ObservableCollection<NonCharacterObject> AllExistingObjects
        {
            get { return _allExistingObjects; }
            set
            {
                if (value != _allExistingObjects)
                {
                    _allExistingObjects = value;
                    RaisePropertyChanged("AllExistingObjects");
                }
            }
        }

        private ObservableCollection<ObjectCategory> _objectCategories;

        public ObservableCollection<ObjectCategory> ObjectCategories
        {
            get { return _objectCategories; }
            set
            {
                if (value != _objectCategories)
                {
                    _objectCategories = value;
                    RaisePropertyChanged("ObjectCategories");
                }
            }
        }

    }
}
