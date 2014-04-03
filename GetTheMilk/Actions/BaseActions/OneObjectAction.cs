namespace GetTheMilk.Actions.BaseActions
{
    public class OneObjectAction:GameAction
    {
        public override bool CanPerform()
        {
            return (TargetObject.AllowsIndirectAction(this, ActiveCharacter)
                    && ActiveCharacter.AllowsIndirectAction(this, TargetObject));
        }


        public override GameAction CreateNewInstance()
        {
            return new OneObjectAction();
        }


    }
}
