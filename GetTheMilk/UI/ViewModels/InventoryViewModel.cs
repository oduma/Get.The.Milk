using System;
using System.Collections.Generic;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace GetTheMilk.UI.ViewModels
{
    public class InventoryViewModel:ViewModelBase
    {
        private readonly ExposeInventoryExtraData _exposeInventoryExtraData;
        public event EventHandler<ActionExecutionRequestEventArgs> ActionExecutionRequest;

        public InventoryViewModel(string ownerName, ExposeInventoryExtraData exposeInventoryExtraData)
        {
            _exposeInventoryExtraData = exposeInventoryExtraData;
            OwnerName = ownerName;
            RefreshInventory();
            Actions= new ObservableCollection<ActionWithTargetModel>();
            Actions.Add(new ActionWithTargetModel{Action=_exposeInventoryExtraData.FinishingAction});

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

        public ObservableCollection<InventoryObjectModel> Tools { get; set; }

        public ObservableCollection<InventoryObjectModel> Weapons { get; set; }

        public ObservableCollection<ActionWithTargetModel> Actions { get; set; }

        private void RefreshInventory()
        {
            Tools = new ObservableCollection<InventoryObjectModel>();
            foreach (ObjectWithPossibleActions tool in _exposeInventoryExtraData.Contents.Where(o => o.Object.ObjectCategory == ObjectCategory.Tool))
            {
                Tools.Add(new InventoryObjectModel(tool, PerformActionCommand));
            }

            Weapons = new ObservableCollection<InventoryObjectModel>();
            foreach (ObjectWithPossibleActions weapon in _exposeInventoryExtraData.Contents.Where(o => o.Object.ObjectCategory == ObjectCategory.Weapon))
            {
                Weapons.Add(new InventoryObjectModel(weapon, PerformActionCommand));
            }

        }

        public void Remove(NonCharacterObject targetObject)
        {
            if (targetObject.ObjectCategory == ObjectCategory.Weapon)
            {
                Weapons.Remove(Weapons.First(
                        o => o.ObjectName == targetObject.Name.Main));
            }
            else if (targetObject.ObjectCategory == ObjectCategory.Tool)
            {
                Tools.Remove(Tools.First(
                        o => o.ObjectName == targetObject.Name.Main));
            }
        }
    }
}
