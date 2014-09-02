﻿using System.Collections.ObjectModel;
using Get.The.Milk.UI.BaseViewModels;
using GetTheMilk.Common;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ObjectHierarchyViewModel:ViewModelBase
    {
        private ObservableCollection<ObjectCategory> _objectCategories;

        public ObservableCollection<ObjectCategory> ObjectCategories
        {
            get { return _objectCategories; }
            set
            {
                if(value!=_objectCategories)
                {
                    _objectCategories = value;
                    RaisePropertyChanged("ActionCategories");
                }
            }
        }

        public ObjectHierarchyViewModel()
        {
            ObjectCategories = 
                new ObservableCollection<ObjectCategory> { ObjectCategory.Decor, ObjectCategory.Tool, ObjectCategory.Weapon };
        }
    }
}
