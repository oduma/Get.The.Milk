using System;
using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Factories;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Actions.ActionTemplates;
using System.Collections.Generic;
using GetTheMilk.BaseCommon;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class InteractionViewModel:ViewModelBase
    {
        private Interaction _value;
        private ObservableCollection<string> _allAvailableActionCategories;

        public ActionViewModel CurrentActionViewModel { get; set; }

        public ActionViewModel CurrentReactionViewModel { get; set; }

        public Interaction Value
        {
            get { return _value; }
            set
            {
                if(value!=_value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        private string _selectedActionCategory;
        public string SelectedActionCategory
        {
            get { return _selectedActionCategory; }
            set
            {
                if (value != _selectedActionCategory)
                {
                    _selectedActionCategory = value;
                    RaisePropertyChanged("SelectedActionCategory");
                    Value.Action = ActionsFactory.CreateAction(_selectedActionCategory);
                    CurrentActionViewModel.Value = Value.Action;
                }
            }
        }

        private string _selectedReactionCategory;

        public string SelectedReactionCategory
        {
            get { return _selectedReactionCategory; }
            set
            {
                if (value != _selectedReactionCategory)
                {
                    _selectedReactionCategory = value;
                    RaisePropertyChanged("SelectedReactionCategory");
                    Value.Reaction = ActionsFactory.CreateAction(_selectedReactionCategory);
                    CurrentReactionViewModel.Value = Value.Reaction;

                }
            }
        }

        public InteractionViewModel(Interaction selectedInteraction)
        {
            if (AllAvailableActionCategories == null)
                AllAvailableActionCategories = new ObservableCollection<string> { CategorysCatalog.MovementCategory,
                    CategorysCatalog.ExposeInventoryCategory,CategorysCatalog.NoObjectCategory,
                    CategorysCatalog.OneObjectCategory,
                    CategorysCatalog.ObjectUseOnObjectCategory,
                    CategorysCatalog.ObjectResponseCategory,
                    CategorysCatalog.TwoCharactersCategory,
                    CategorysCatalog.ObjectTransferCategory};
            Value = selectedInteraction;
            if (CurrentActionViewModel == null)
                CurrentActionViewModel = new ActionViewModel();
            if (CurrentReactionViewModel == null)
                CurrentReactionViewModel = new ActionViewModel();
            if (selectedInteraction !=null && selectedInteraction.Action != null)
            {
                _selectedActionCategory =selectedInteraction.Action.Category;
                RaisePropertyChanged("SelectedActionCategory");
                CurrentActionViewModel.Value = selectedInteraction.Action;
            }
            if (selectedInteraction != null && selectedInteraction.Reaction != null)
            {
                _selectedReactionCategory =selectedInteraction.Reaction.Category;
                RaisePropertyChanged("SelectedReactionCategory");
                CurrentReactionViewModel.Value = selectedInteraction.Reaction;
            }
        }

        public ObservableCollection<string> AllAvailableActionCategories
        {
            get { return _allAvailableActionCategories; }
            set
            {
                if(value!=_allAvailableActionCategories)
                {
                    _allAvailableActionCategories = value;
                    RaisePropertyChanged("AllAvailableActionCategories");
                }
            }
        }

        internal void UpdateValue()
        {
            Value.Action = CurrentActionViewModel.Value;
            Value.Reaction = CurrentReactionViewModel.Value;
        }
    }
}
