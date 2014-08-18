using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Factories;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
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
                    RaisePropertyChanged("Value");
                    DisplayDetails(value);
                }
            }
        }

        public IEnumerable<Type> AllPerformerTypes
        {
            get 
            {
                return TemplatedActionPerformersFactory.GetFactory().ListAllActionPerformerTypes();
            }
        }

        private void DisplayDetails(BaseActionTemplate value)
        {
            if (value.Category == CategorysCatalog.ExposeInventoryCategory)
                CurrentActionDetailsViewModel = new ExposeInventoryActionViewModel();
            else if (value.Category == CategorysCatalog.MovementCategory)
                CurrentActionDetailsViewModel = new MovementActionViewModel();
            else if (value.Category == CategorysCatalog.ObjectUseOnObjectCategory)
                CurrentActionDetailsViewModel = new ObjectUseOnObjectActionViewModel();
            else if (value.Category == CategorysCatalog.TwoCharactersCategory)
                CurrentActionDetailsViewModel = new TwoCharactersActionViewModel();
               
        }

    }
}
