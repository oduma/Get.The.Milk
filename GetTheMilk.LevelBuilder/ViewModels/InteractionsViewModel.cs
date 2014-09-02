using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.UI.ViewModels.BaseViewModels;
using GetTheMilk.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class InteractionsViewModel:ViewModelBase
    {

        public InteractionsViewModel(IActionEnabled value,
            ObservableCollection<Interaction> allAvailableInteractions)
        {
            if(value.Interactions==null)
            value.Interactions= new SortedList<string, Interaction[]>();
                    AllAvailableInteractions = allAvailableInteractions;

            if(AnyCharacterInteractions==null)
                AnyCharacterInteractions= new ObservableCollection<Interaction>();

            if(value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacter))
                foreach( var car in value.Interactions[GenericInteractionRulesKeys.AnyCharacter])
                    AnyCharacterInteractions.Add(car);

            if (AnyCharacterResponseInteractions == null)
                AnyCharacterResponseInteractions = new ObservableCollection<Interaction>();

            if(value.Interactions.ContainsKey(GenericInteractionRulesKeys.AnyCharacterResponses))
                foreach (var car in value.Interactions[GenericInteractionRulesKeys.AnyCharacterResponses])
                    AnyCharacterResponseInteractions.Add(car);

            MoveToAnyCharacterInteractions=new RelayCommand(MoveToAnyCharacterInteractionsCommand);
            RemoveFromAnyCharacterInteractions=new RelayCommand(RemoveFromAnyCharacterInteractionsCommand);
            MoveToAnyCharacterResponseInteractions=new RelayCommand(MoveToAnyCharacterResponseInteractionsCommand);
            RemoveFromAnyCharacterResponseInteractions=new RelayCommand(RemoveFromAnyCharacterResponseInteractionsCommand);
        }

        public RelayCommand MoveToAnyCharacterInteractions { get; private set; }
        public RelayCommand RemoveFromAnyCharacterInteractions { get; private set; }
        public RelayCommand MoveToAnyCharacterResponseInteractions { get; private set; }
        public RelayCommand RemoveFromAnyCharacterResponseInteractions { get; private set; }

        private void RemoveFromAnyCharacterResponseInteractionsCommand()
        {
            AllAvailableInteractions.Add(SelectedAnyCharacterResponseInteraction);
            AnyCharacterResponseInteractions.Remove(SelectedAnyCharacterResponseInteraction);
        }

        public Interaction SelectedAnyCharacterResponseInteraction
        {
            get { return _selectedAnyCharacterResponseInteraction; }
            set
            {
                if(value!=_selectedAnyCharacterResponseInteraction)
                {
                    _selectedAnyCharacterResponseInteraction = value;
                    RaisePropertyChanged("SelectedAnyCharacterResponseInteraction");
                }
            }
        }

        private void MoveToAnyCharacterResponseInteractionsCommand()
        {
            AnyCharacterResponseInteractions.Add(SelectedAvailableInteraction);
            AllAvailableInteractions.Remove(SelectedAvailableInteraction);
        }

        public Interaction SelectedAvailableInteraction
        {
            get { return _selectedAvailableInteraction; }
            set
            {
                if(value!=_selectedAvailableInteraction)
                {
                    _selectedAvailableInteraction = value;
                    RaisePropertyChanged("SelectedAvailableInteraction");
                }
            }
        }

        private void RemoveFromAnyCharacterInteractionsCommand()
        {
            AllAvailableInteractions.Add(SelectedAnyCharacterInteraction);
            AnyCharacterInteractions.Remove(SelectedAnyCharacterInteraction);
        }

        public Interaction SelectedAnyCharacterInteraction
        {
            get { return _selectedAnyCharacterInteraction; }
            set
            {
                if(value!=_selectedAnyCharacterInteraction)
                {
                    _selectedAnyCharacterInteraction = value;
                    RaisePropertyChanged("SelectedAnyCharacterInteraction");
                }
            }
        }

        private void MoveToAnyCharacterInteractionsCommand()
        {
            AnyCharacterInteractions.Add(SelectedAvailableInteraction);
            AllAvailableInteractions.Remove(SelectedAvailableInteraction);
        }

        public ObservableCollection<Interaction> AllAvailableInteractions
        {
            get { return _allAvailableInteractions; }
            set
            {
                if(value!=_allAvailableInteractions)
                {
                    _allAvailableInteractions = value;
                    RaisePropertyChanged("AllAvailableInteractions");
                }
            }
        }

        private ObservableCollection<Interaction> _allAvailableInteractions;
        private Interaction _selectedAvailableInteraction;
        private Interaction _selectedAnyCharacterInteraction;
        private Interaction _selectedAnyCharacterResponseInteraction;
        private ObservableCollection<Interaction> _anyCharacterInteractions;
        private ObservableCollection<Interaction> _anyCharacterResponseInteractions;

        public ObservableCollection<Interaction> AnyCharacterInteractions
        {
            get { return _anyCharacterInteractions; }

            set
            {
                if (_anyCharacterInteractions != value)
                {
                    _anyCharacterInteractions = value;
                    RaisePropertyChanged("AnyCharacterInteractions");
                }
            }
        }

        public ObservableCollection<Interaction> AnyCharacterResponseInteractions
        {
            get { return _anyCharacterResponseInteractions; }

            set
            {
                if (_anyCharacterResponseInteractions != value)
                {
                    _anyCharacterResponseInteractions = value;
                    RaisePropertyChanged("AnyCharacterResponseInteractions");
                }
            }
        }

    }
}
