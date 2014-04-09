using System;
using GetTheMilk.Actions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace GetTheMilk.UI.ViewModels
{
    public class InventoryViewModel:ViewModelBase
    {
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        public InventoryViewModel(string ownerName, ExposeInventoryExtraData exposeInventoryExtraData)
        {
            OwnerName = ownerName;
            Tools = new ObservableCollection<ObjectWithPossibleActions>();
            foreach (ObjectWithPossibleActions tool in exposeInventoryExtraData.Contents.Where(o => o.Object.ObjectCategory == ObjectCategory.Tool))
            {
                Tools.Add(tool);
            }
            Weapons = new ObservableCollection<ObjectWithPossibleActions>();
            foreach (ObjectWithPossibleActions weapon in exposeInventoryExtraData.Contents.Where(o => o.Object.ObjectCategory == ObjectCategory.Weapon))
            {
                Weapons.Add(weapon);
            }

            Actions= new ObservableCollection<ActionWithTargetModel>();
            Actions.Add(new ActionWithTargetModel{Action=new CloseInventory()});

            PerformAction = new RelayCommand<ActionWithTargetModel>(PerformActionCommand);

        }

        private void PerformActionCommand(ActionWithTargetModel obj)
        {
            if (ActionExecutionRequest != null)
                ActionExecutionRequest(this, new ActionExecutionRequestEventArgs(obj.Action));
        }

        public RelayCommand<ActionWithTargetModel> PerformAction { get; private set; }

        private string _ownerName;

        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                if (value != _ownerName)
                {
                    _ownerName = value;
                    RaisePropertyChanged("OwnerName");
                }

            }
        }

        public ObservableCollection<ObjectWithPossibleActions> Tools { get; set; }

        public ObservableCollection<ObjectWithPossibleActions> Weapons { get; set; }

        public ObservableCollection<ActionWithTargetModel> Actions { get; set; }

    }
}
