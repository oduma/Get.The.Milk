using System.Collections.ObjectModel;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class DecorObjectViewModel:ObjectViewModelBase<NonCharacterObject>
    {
        public DecorObjectViewModel(NonCharacterObject nonCharacter)
        {
            if(nonCharacter.Name==null)
                nonCharacter.Name=new Noun();
            Value = nonCharacter;
            var objectTypes = ObjectActionsFactory.GetFactory().ListAllRegisterNames(ObjectCategory.Decor);
            if(AllObjectTypes==null)
                AllObjectTypes= new ObservableCollection<string>();
            foreach (var objectType in objectTypes)
                AllObjectTypes.Add(objectType);
        }

        private NonCharacterObject _value;
        public override NonCharacterObject Value
        {
            get { return _value; }
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
                    AllowsAction = Value.AllowsAction,
                    AllowsIndirectAction = Value.AllowsIndirectAction,
                    BlockMovement = Value.BlockMovement,
                    CloseUpMessage = Value.CloseUpMessage,
                    Name =
                        new Noun
                        {
                            Main = Value.Name.Main,
                            Narrator = Value.Name.Narrator
                        },
                    ObjectTypeId = Value.ObjectTypeId,
                });
        }
    }
}
