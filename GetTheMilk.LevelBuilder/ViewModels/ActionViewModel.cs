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
        private ViewModelBase _currentActionDetailsViewModel;

        public ViewModelBase CurrentActionDetailsViewModel
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

        private BaseActionTemplate _value;

        public BaseActionTemplate Value 
        { 
            get 
            { 
                return _value; 
            }
            set 
            {
                if(value!=_value)
                {
                    _value = value;
                    if (_value.Name != null)
                    {
                        UniqueId = _value.Name.UniqueId;
                    }
                    RaisePropertyChanged("Value");
                    DisplayDetails(value);
                }
            }
        }

        private ObservableCollection<Type> _allPerformerTypes;
        public ObservableCollection<Type> AllPerformerTypes
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
                CurrentActionDetailsViewModel = new ExposeInventoryActionViewModel(Value as ExposeInventoryActionTemplate);
            else if (value.Category == CategorysCatalog.MovementCategory)
                CurrentActionDetailsViewModel = new MovementActionViewModel(Value as MovementActionTemplate);
            else if (value.Category == CategorysCatalog.ObjectUseOnObjectCategory)
                CurrentActionDetailsViewModel = new ObjectUseOnObjectActionViewModel(Value as ObjectUseOnObjectActionTemplate);
            else if (value.Category == CategorysCatalog.TwoCharactersCategory)
                CurrentActionDetailsViewModel = new TwoCharactersActionViewModel(Value as TwoCharactersActionTemplate);
            if (Value != null && Value.Category != null)
            {
                var tempPerf = TemplatedActionPerformersFactory.GetFactory()
                    .GetAllActionPerformers()
                    .Where(p => p.Category == Value.Category && p is BaseActionResponsePerformer).Select(p => p.GetType());
                if (AllPerformerTypes == null)
                    AllPerformerTypes = new ObservableCollection<Type>();
                foreach (var perf in tempPerf)
                    AllPerformerTypes.Add(perf);
            }
            else
                AllPerformerTypes = null;

               
        }

    }
}
