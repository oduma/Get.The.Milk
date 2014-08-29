using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ActionViewModel:ViewModelBase
    {
        private ActionDetailViewModelBase _currentActionDetailsViewModel;

        public ActionDetailViewModelBase CurrentActionDetailsViewModel
        {
            get { return _currentActionDetailsViewModel; }
            set
            {
                if (value != _currentActionDetailsViewModel)
                {
                    _currentActionDetailsViewModel = value;
                    RaisePropertyChanged("CurrentActionDetailsViewModel");
                }
            }

        }

        private string _uniqueId;

        public string UniqueId
        {
            get
            {
                return _uniqueId;
            }
            set
            {
                if (value != _uniqueId)
                {
                    _uniqueId = value;
                    RaisePropertyChanged("UniqueId");
                }
            }
        }

        private string _past;

        public string Past
        {
            get
            {
                return _past;
            }
            set
            {
                if (value != _past)
                {
                    _past = value;
                    RaisePropertyChanged("Past");
                }
            }
        }

        private string _present;

        public string Present
        {
            get
            {
                return _present;
            }
            set
            {
                if (value != _present)
                {
                    _present = value;
                    RaisePropertyChanged("Present");
                }
            }
        }

        private bool _startingAction;

        public bool StartingAction
        {
            get
            {
                return _startingAction;
            }
            set
            {
                if (value != _startingAction)
                {
                    _startingAction = value;
                    RaisePropertyChanged("StartingAction");
                }
            }
        }

        private string _performerType;

        public string PerformerType
        {
            get
            {
                return _performerType;
            }
            set
            {
                if (value != _performerType)
                {
                    _performerType = value;
                    RaisePropertyChanged("PerformerType");
                }
            }
        }

        private BaseActionTemplate _value;

        public BaseActionTemplate Value 
        { 
            get 
            {
                if (string.IsNullOrEmpty(UniqueId))
                {
                    return null;
                }
                _value.Name.UniqueId = UniqueId;
                _value.Name.Past = Past;
                _value.Name.Present = Present;
                _value.StartingAction = StartingAction;
                _value.CurrentPerformer = TemplatedActionPerformersFactory.CreateActionPerformer(PerformerType);
                if(CurrentActionDetailsViewModel!=null)
                    CurrentActionDetailsViewModel.ApplyDetailsToValue(ref _value);
                return _value;
            }
            set 
            {
                _value = value;
                if (_value.Name != null)
                {
                    UniqueId = _value.Name.UniqueId;
                    Past = _value.Name.Past;
                    Present = _value.Name.Present;
                }
                StartingAction = _value.StartingAction;
                if(_value.PerformerType!=null)
                    PerformerType = _value.PerformerType.Name;
                DisplayDetails(value);
            }
        }

        private ObservableCollection<string> _allPerformerTypes;
        public ObservableCollection<string> AllPerformerTypes
        {
            set
            {
                if(value!=_allPerformerTypes)
                {
                    _allPerformerTypes = value;
                    RaisePropertyChanged("AllPerformerTypes");
                }
            }
            get 
            {
                return _allPerformerTypes;
            }
        }

        private void DisplayDetails(BaseActionTemplate value)
        {
            if (value.Name == null)
                value.Name = new Verb();
            if (value.Category == CategorysCatalog.ExposeInventoryCategory)
                CurrentActionDetailsViewModel = new ExposeInventoryActionViewModel(value as ExposeInventoryActionTemplate);
            else if (value.Category == CategorysCatalog.MovementCategory)
                CurrentActionDetailsViewModel = new MovementActionViewModel(value as MovementActionTemplate);
            else if (value.Category == CategorysCatalog.ObjectUseOnObjectCategory)
                CurrentActionDetailsViewModel = new ObjectUseOnObjectActionViewModel(value as ObjectUseOnObjectActionTemplate);
            else if (value.Category == CategorysCatalog.TwoCharactersCategory)
                CurrentActionDetailsViewModel = new TwoCharactersActionViewModel(value as TwoCharactersActionTemplate);
            if (value != null && value.Category != null)
            {
                var tempPerf = TemplatedActionPerformersFactory.GetAllActionPerformers()
                    .Where(p => p.Category == value.Category).Select(p => p.GetType().Name);
                if (AllPerformerTypes == null)
                    AllPerformerTypes = new ObservableCollection<string>();
                foreach (var perf in tempPerf)
                    AllPerformerTypes.Add(perf);
            }
            else
                AllPerformerTypes = null;

               
        }
    }
}
