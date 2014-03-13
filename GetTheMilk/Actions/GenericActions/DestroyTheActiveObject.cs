using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.GenericActions
{
    public class DestroyTheActiveObject:ObjectUseOnObjectAction
    {

        public override void Perform(ref NonCharacterObject a, ref NonCharacterObject p)
        {
            a.StorageContainer.Remove(a);
            a = null;
        }
    }
}
