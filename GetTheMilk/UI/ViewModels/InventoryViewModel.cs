using GetTheMilk.Actions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.UI.ViewModels
{
    public class InventoryViewModel:ViewModelBase
    {
        public InventoryViewModel(string ownerName, ExposeInventoryExtraData exposeInventoryExtraData)
        {
            OwnerName = ownerName;
            Tools = new ObservableCollection<Tool>();
            foreach (Tool tool in exposeInventoryExtraData.Contents.Where(o => o.ObjectCategory == ObjectCategory.Tool))
            {
                Tools.Add(tool);
            }
        }

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

        public ObservableCollection<Tool> Tools { get; set; }

    }
}
