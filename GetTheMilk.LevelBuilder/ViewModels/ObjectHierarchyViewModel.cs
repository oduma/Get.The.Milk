﻿using System.Collections.ObjectModel;
using GetTheMilk.Common;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

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
