using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class InteractionManagerViewModel:ViewModelBase
    {
        private InteractionViewModel _currentInteractionViewModel;

        public InteractionViewModel CurrentInteractionViewModel
        {
            get { return _currentInteractionViewModel; }
            set
            {
                if (value != _currentInteractionViewModel)
                {
                    _currentInteractionViewModel = value;
                    RaisePropertyChanged("CurrentInteractionViewModel");
                }
            }

        }


        public RelayCommand CreateNewInteraction { get; private set; }
        public RelayCommand Done { get; private set; }

        public InteractionManagerViewModel()
        {
            CreateNewInteraction= new RelayCommand(DisplayNewInteractionEditor);
            Done= new RelayCommand(DoneCommand,CanBeDone);
        }

        private bool CanBeDone()
        {
            return CurrentInteractionViewModel!=null && (CurrentInteractionViewModel.Value.Action != null &&
                    CurrentInteractionViewModel.Value.Reaction != null);
        }

        private void DispalyInteractionEditor()
        {
            if(SelectedInteraction!=null)
                CurrentInteractionViewModel = new InteractionViewModel(SelectedInteraction);
        }

        private void DoneCommand()
        {
            if(AllExistingInteractions==null)
                AllExistingInteractions=new ObservableCollection<Interaction>();

            var existingInteraction =
                AllExistingInteractions.FirstOrDefault(i => i.ToString() == CurrentInteractionViewModel.Value.ToString());
            if (existingInteraction != null)
                AllExistingInteractions.Remove(existingInteraction);
            AllExistingInteractions.Add(CurrentInteractionViewModel.Value);
            DisplayNewInteractionEditor();
        }

        private void DisplayNewInteractionEditor()
        {
            CurrentInteractionViewModel = new InteractionViewModel(new Interaction());
        }

        private Interaction _selectedInteraction;
        public Interaction SelectedInteraction
        {
            get { return _selectedInteraction; }
            set
            {
                if (value != _selectedInteraction)
                {
                    _selectedInteraction = value;
                    RaisePropertyChanged("SelectedInteraction");
                    DispalyInteractionEditor();
                }
            }
        }
        private ObservableCollection<Interaction> _allExistingInteractions;

        public ObservableCollection<Interaction> AllExistingInteractions
        {
            get { return _allExistingInteractions; }
            set
            {
                if (value != _allExistingInteractions)
                {
                    _allExistingInteractions = value;
                    RaisePropertyChanged("AllExistingInteractions");
                }
            }
        }



    }
}
