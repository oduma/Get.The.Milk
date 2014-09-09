using System;
using System.Collections.ObjectModel;
using System.IO;
using Get.The.Milk.UI.BaseViewModels;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using System.Collections.Generic;
using GetTheMilk.Utils;
using System.Linq;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public abstract class ObjectViewModelBase<T>:ViewModelBase where T:IActionEnabled
    {
        public abstract T Value { get; set; }

        private ObservableCollection<string> _allObjectTypes;
        public ObservableCollection<string> AllObjectTypes
        {
            get { return _allObjectTypes; }
            set
            {
                if (value != _allObjectTypes)
                {
                    _allObjectTypes = value;
                    RaisePropertyChanged("AllObjectTypes");
                }
            }
        }

        private ObservableCollection<string> _allAvailableSprites;
        public ObservableCollection<string> AllAvailableSprites
        {
            get { return _allAvailableSprites; }
            set
            {
                if (value != _allAvailableSprites)
                {
                    _allAvailableSprites = value;
                    RaisePropertyChanged("AllAvailableSprites");
                }
            }
        }

        public string SpriteName
        {
            get { return Value.SpriteName; }
            set
            {
                if (value != Value.SpriteName)
                {
                    Value.SpriteName = value;
                    RaisePropertyChanged("SpriteName");
                }
            }
        }

        private InteractionsViewModel _currentInteractionsViewModel;
        public InteractionsViewModel CurrentInteractionsViewModel
        {
            get { return _currentInteractionsViewModel; }
            set
            {
                if (value != _currentInteractionsViewModel)
                {
                    _currentInteractionsViewModel = value;
                    RaisePropertyChanged("CurrentInteractionsiewModel");
                }
            }
        }

        public abstract ObjectViewModelBase<T> Clone();

        internal void RefreshInteractions()
        {
            if(Value.Interactions==null)
                Value.Interactions=new SortedList<string,Interaction[]>();
            if (Value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacter))
                Value.Interactions.Remove(GenericInteractionRulesKeys.AnyCharacter);
            if (Value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
                Value.Interactions.Remove(GenericInteractionRulesKeys.AnyCharacterResponses);
            if(CurrentInteractionsViewModel.AnyCharacterInteractions!=null 
                && CurrentInteractionsViewModel.AnyCharacterInteractions.Any())
                Value.Interactions.Add(GenericInteractionRulesKeys.AnyCharacter, 
                    CurrentInteractionsViewModel.AnyCharacterInteractions.ToArray());
            if(CurrentInteractionsViewModel.AnyCharacterResponseInteractions!=null 
                && CurrentInteractionsViewModel.AnyCharacterResponseInteractions.Any())
                Value.Interactions.Add(GenericInteractionRulesKeys.AnyCharacterResponses, 
                    CurrentInteractionsViewModel.AnyCharacterResponseInteractions.ToArray());

        }

        protected ObjectViewModelBase()
        {
            _allAvailableSprites= new ObservableCollection<string>();
            foreach (
                var sprite in
                    Directory.GetFiles(@"ImageLibrary", "*.png").Select(f => Path.GetFileName(f).Replace(".png", "")))
            {
                _allAvailableSprites.Add(sprite);
            }
        }
    }
}
