using Get.The.Milk.UI.BaseViewModels;
using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ActionManagerViewModel:ViewModelBase
    {

        //private ObjectViewModelBase<NonCharacterObject> _currentObjectViewModel;

        //public ObjectViewModelBase<NonCharacterObject> CurrentObjectViewModel
        //{
        //    get { return _currentObjectViewModel; }
        //    set
        //    {
        //        if (value != _currentObjectViewModel)
        //        {
        //            _currentObjectViewModel = value;
        //            RaisePropertyChanged("CurrentObjectViewModel");
        //        }
        //    }

        //}

        public RelayCommand<string> CreateNewAction { get; private set; }
        public RelayCommand Done { get; private set; }

        public ActionManagerViewModel()
        {
            ActionCategories = 
                new ObservableCollection<string> { CategorysCatalog.MovementCategory,
                    CategorysCatalog.ExposeInventoryCategory,CategorysCatalog.NoObjectCategory,
                    CategorysCatalog.OneObjectCategory,CategorysCatalog.ObjectTransferCategory,
                    CategorysCatalog.ObjectUseOnObjectCategory,CategorysCatalog.ObjectResponseCategory,
                    CategorysCatalog.TwoCharactersCategory};
            CreateNewAction= new RelayCommand<string>(DisplayNewActionEditor);
            Done= new RelayCommand(DoneCommand);
        }

        private void DispalyActionEditor()
        {

            //switch(SelectedAction.Category)
            //{
            //    case ObjectCategory.Decor:

            //        CurrentObjectViewModel = new DecorObjectViewModel(SelectedAction, _allAvailableInteractions);
            //        break;
            //    case ObjectCategory.Tool:
            //        CurrentObjectViewModel = new ToolObjectViewModel(SelectedAction as Tool, _allAvailableInteractions);
            //        break;
            //    case ObjectCategory.Weapon:
            //        CurrentObjectViewModel = new WeaponObjectViewModel(SelectedAction as Weapon, _allAvailableInteractions);
            //        break;
            //}

        }

        private void DoneCommand()
        {
            //if(AllExistingActions==null)
            //    AllExistingActions=new ObservableCollection<BaseActionTemplate>();
            //if (AllExistingActions.Any(c => c.Name.UniqueId == CurrentObjectViewModel.Value.Name.Main))
            //    AllExistingActions.Remove(
            //        AllExistingActions.First(c => c.Name.Main == CurrentObjectViewModel.Value.Name.Main));

            //AllExistingActions.Add(CurrentObjectViewModel.Value);
            //DisplayNewObjectEditor(CurrentObjectViewModel.Value.ObjectCategory);
        }

        private void DisplayNewActionEditor(string act)
        {
            //switch(act)
            //{
            //    case ObjectCategory.Decor:

            //        CurrentObjectViewModel = new DecorObjectViewModel(new NonCharacterObject(), _allAvailableInteractions);
            //        break;
            //        case ObjectCategory.Tool:
            //        CurrentObjectViewModel = new ToolObjectViewModel(new Tool(), _allAvailableInteractions);
            //        break;
            //        case ObjectCategory.Weapon:
            //        CurrentObjectViewModel = new WeaponObjectViewModel(new Weapon(),_allAvailableInteractions);
            //        break;
            //}
        }

        private BaseActionTemplate _selectedAction;
        public BaseActionTemplate SelectedAction
        {
            get { return _selectedAction; }
            set
            {
                if (value != _selectedAction)
                {
                    _selectedAction = value;
                    RaisePropertyChanged("SelectedAction");
                    if(_selectedAction!=null)
                        DispalyActionEditor();
                }
            }
        }
        private ObservableCollection<BaseActionTemplate> _allExistingActions;
        public ObservableCollection<BaseActionTemplate> AllExistingActions
        {
            get { return _allExistingActions; }
            set
            {
                if (value != _allExistingActions)
                {
                    _allExistingActions = value;
                    RaisePropertyChanged("AllExistingActions");
                }
            }
        }

        private ObservableCollection<string> _actionCategories;

        public ObservableCollection<string> ActionCategories
        {
            get { return _actionCategories; }
            set
            {
                if (value != _actionCategories)
                {
                    _actionCategories = value;
                    RaisePropertyChanged("ActionCategories");
                }
            }
        }



    }
}
