using System.Collections.ObjectModel;
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
            Done= new RelayCommand(DoneCommand);
        }

        private void DispalyInteractionEditor()
        {
            CurrentInteractionViewModel = new InteractionViewModel(SelectedInteraction);
        }

        private void DoneCommand()
        {
            if(AllExistingInteractions==null)
                AllExistingInteractions=new ObservableCollection<ActionReaction>();
            AllExistingInteractions.Add(CurrentInteractionViewModel.Value);
            DisplayNewInteractionEditor();
        }

        private void DisplayNewInteractionEditor()
        {
            CurrentInteractionViewModel = new InteractionViewModel(new ActionReaction());
        }

        private ActionReaction _selectedInteraction;
        public ActionReaction SelectedInteraction
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
        private ObservableCollection<ActionReaction> _allExistingInteractions;

        public ObservableCollection<ActionReaction> AllExistingInteractions
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
