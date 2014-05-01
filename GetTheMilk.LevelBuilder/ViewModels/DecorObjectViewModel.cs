using GetTheMilk.BaseCommon;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class DecorObjectViewModel:ObjectViewModelBase
    {
        public DecorObjectViewModel(NonCharacterObject nonCharacter)
        {
            if(nonCharacter.Name==null)
                nonCharacter.Name=new Noun();
            Value = nonCharacter;
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


    }
}
