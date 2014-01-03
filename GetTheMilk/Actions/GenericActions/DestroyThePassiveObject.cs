using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.GenericActions
{
    public abstract class DestroyThePassiveObject:ObjectUseOnObjectAction
    {
        public override void Perform(ref IPositionableObject a, ref IPositionableObject p)
        {
                p.StorageContainer.Remove(p);
                p = null;
        }
    }
}
