using System;
using System.Collections.Generic;
using GetTheMilk.Actions.ActionTemplates;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class InventoryObjectModel:ViewModelBase
    {
        private readonly Action<ActionWithTargetModel> _performActionCommand;
        public string ObjectName { get; private set; }

        public IEnumerable<ActionWithTargetModel> Actions { get; private set; }

        public InventoryObjectModel(ObjectWithPossibleActionTemplates o, Action<ActionWithTargetModel> performActionCommand)
        {
            _performActionCommand = performActionCommand;
            ObjectName = o.Object.Name.Main;
            Actions = (o.PossibleUsses!=null)?o.PossibleUsses.Select(possibleUse => new ActionWithTargetModel { Action = possibleUse }):null;
            PerformAction = new RelayCommand<ActionWithTargetModel>(_performActionCommand);

        }

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }


    }
}
