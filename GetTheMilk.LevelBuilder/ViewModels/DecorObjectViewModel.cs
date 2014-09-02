using System.Collections.ObjectModel;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class DecorObjectViewModel:ObjectViewModelBase<NonCharacterObject>
    {
        public DecorObjectViewModel(NonCharacterObject nonCharacter,
            ObservableCollection<Interaction> allAvailableInteractions)
        {
            if(nonCharacter.Name==null)
                nonCharacter.Name=new Noun();
            Value = nonCharacter;
            CurrentInteractionsViewModel = new InteractionsViewModel(Value, allAvailableInteractions);
            var objectTypes = ObjectActionsFactory.ListAllRegisterNames(ObjectCategory.Decor);
            if(AllObjectTypes==null)
                AllObjectTypes= new ObservableCollection<string>();
            foreach (var objectType in objectTypes)
                AllObjectTypes.Add(objectType);
        }

        private NonCharacterObject _value;
        public override NonCharacterObject Value
        {
            get 
            { 
                return _value; 
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public override ObjectViewModelBase<NonCharacterObject> Clone()
        {
            return
                new DecorObjectViewModel(new NonCharacterObject
                {
                    AllowsTemplateAction = Value.AllowsTemplateAction,
                    AllowsIndirectTemplateAction = Value.AllowsIndirectTemplateAction,
                    BlockMovement = Value.BlockMovement,
                    Name =
                        new Noun
                        {
                            Main = Value.Name.Main,
                            Narrator = Value.Name.Narrator,
                            Description=Value.Name.Description
                        },
                    ObjectTypeId = Value.ObjectTypeId,
                    Interactions=Value.Interactions
                },CurrentInteractionsViewModel.AllAvailableInteractions);
        }
    }
}
