using System.Collections.ObjectModel;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Factories;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class ToolObjectViewModel:ObjectViewModelBase<NonCharacterObject>
    {
        public ToolObjectViewModel(Tool tool, ObservableCollection<Interaction> allAvailableInteractions)
        {
            if(tool.Name==null)
                tool.Name=new Noun();
            Value = tool;
            CurrentInteractionsViewModel = new InteractionsViewModel(Value,allAvailableInteractions);
            var objectTypes = ObjectActionsFactory.ListAllRegisterNames(ObjectCategory.Tool);
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
                    AllowsTemplateAction = Value.AllowsTemplateAction,
                    AllowsIndirectTemplateAction = Value.AllowsIndirectTemplateAction,
                    BlockMovement = ((Tool)Value).BlockMovement,
                    BuyPrice = ((Tool)Value).BuyPrice,
                    Name =
                        new Noun
                        {
                            Main = ((Tool)Value).Name.Main,
                            Narrator = ((Tool)Value).Name.Narrator,
                            Description= Value.Name.Description
                        },
                    ObjectTypeId = ((Tool)Value).ObjectTypeId,
                    SellPrice = ((Tool)Value).SellPrice,
                    Interactions=Value.Interactions
                }, CurrentInteractionsViewModel.AllAvailableInteractions);
        }
    }
}
