using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ObjectManagerViewModel:ViewModelBase
    {
        private ObjectViewModelBase<NonCharacterObject> _currentObjectViewModel;

        public ObjectViewModelBase<NonCharacterObject> CurrentObjectViewModel
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

        private int _noOfInstances;

        public int NoOfInstances
        {
            get { return _noOfInstances; }
            set
            {
                if (value != _noOfInstances)
                {
                    _noOfInstances = value;
                    RaisePropertyChanged("NoOfInstances");
                }
            }

        }


        public RelayCommand<ObjectCategory> CreateNewObject { get; private set; }
        public RelayCommand Done { get; private set; }
        public RelayCommand CreateInstances { get; private set; }

        public ObjectManagerViewModel()
        {
            ObjectCategories = 
                new ObservableCollection<ObjectCategory> { ObjectCategory.Decor, ObjectCategory.Tool, ObjectCategory.Weapon };
            CreateNewObject= new RelayCommand<ObjectCategory>(DisplayNewObjectEditor);
            NoOfInstances = 1;
            Done= new RelayCommand(DoneCommand);
            CreateInstances= new RelayCommand(CreateInstancesCommand);
        }

        private void CreateInstancesCommand()
        {
            if (AllExistingObjects == null)
                AllExistingObjects = new ObservableCollection<NonCharacterObject>();
            for (int i = 1; i < NoOfInstances + 1;i++ )
            {
                var newObjectViewModel = CurrentObjectViewModel.Clone();
                newObjectViewModel.Value.Name.Main = string.Format("{0} {1}", CurrentObjectViewModel.Value.Name.Main,
                                                                       i);
                if (AllExistingObjects.Any(c => c.Name.Main == newObjectViewModel.Value.Name.Main))
                    AllExistingObjects.Remove(
                        AllExistingObjects.First(c => c.Name.Main == newObjectViewModel.Value.Name.Main));

                AllExistingObjects.Add(newObjectViewModel.Value);
            }
            DisplayNewObjectEditor(CurrentObjectViewModel.Value.ObjectCategory);
        }

        private void DispalyObjectEditor()
        {
            switch(SelectedObject.ObjectCategory)
            {
                case ObjectCategory.Decor:

                    CurrentObjectViewModel = new DecorObjectViewModel(SelectedObject);
                    break;
                case ObjectCategory.Tool:
                    CurrentObjectViewModel = new ToolObjectViewModel(SelectedObject as Tool);
                    break;
                case ObjectCategory.Weapon:
                    CurrentObjectViewModel = new WeaponObjectViewModel(SelectedObject as Weapon);
                    break;
            }

        }

        private void DoneCommand()
        {
            if(AllExistingObjects==null)
                AllExistingObjects=new ObservableCollection<NonCharacterObject>();
            if (AllExistingObjects.Any(c => c.Name.Main == CurrentObjectViewModel.Value.Name.Main))
                AllExistingObjects.Remove(
                    AllExistingObjects.First(c => c.Name.Main == CurrentObjectViewModel.Value.Name.Main));

            AllExistingObjects.Add(CurrentObjectViewModel.Value);
            DisplayNewObjectEditor(CurrentObjectViewModel.Value.ObjectCategory);
        }

        private void DisplayNewObjectEditor(ObjectCategory obj)
        {
            switch(obj)
            {
                case ObjectCategory.Decor:
                    
                    CurrentObjectViewModel = new DecorObjectViewModel(new NonCharacterObject());
                    break;
                    case ObjectCategory.Tool:
                    CurrentObjectViewModel = new ToolObjectViewModel(new Tool());
                    break;
                    case ObjectCategory.Weapon:
                    CurrentObjectViewModel = new WeaponObjectViewModel(new Weapon());
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
                    if(_selectedObject!=null)
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
