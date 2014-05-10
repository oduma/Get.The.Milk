using System.Collections.ObjectModel;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ToolObjectViewModel:ObjectViewModelBase<NonCharacterObject>
    {
        public ToolObjectViewModel(Tool tool)
        {
            if(tool.Name==null)
                tool.Name=new Noun();
            Value = tool;
            var objectTypes = ObjectActionsFactory.GetFactory().ListAllRegisterNames(ObjectCategory.Tool);
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
                new ToolObjectViewModel(new Tool
                {
                    AllowsAction = Value.AllowsAction,
                    AllowsIndirectAction = Value.AllowsIndirectAction,
                    BlockMovement = ((Tool)Value).BlockMovement,
                    BuyPrice = ((Tool)Value).BuyPrice,
                    CloseUpMessage = ((Tool)Value).CloseUpMessage,
                    Name =
                        new Noun
                        {
                            Main = ((Tool)Value).Name.Main,
                            Narrator = ((Tool)Value).Name.Narrator
                        },
                    ObjectTypeId = ((Tool)Value).ObjectTypeId,
                    SellPrice = ((Tool)Value).SellPrice
                });
        }
    }
}
