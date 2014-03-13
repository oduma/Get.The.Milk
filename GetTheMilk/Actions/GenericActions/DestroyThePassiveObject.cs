using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.GenericActions
{
    public class DestroyThePassiveObject:ObjectUseOnObjectAction
    {
        public override void Perform(ref NonCharacterObject a, ref NonCharacterObject p)
        {
                p.StorageContainer.Remove(p);
                p = null;
        }
    }
}
