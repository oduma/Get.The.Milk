using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.GenericActions
{
    public abstract class DestroyBothObjects:ObjectUseOnObjectAction
    {
        public override void Perform(ref IPositionableObject a, ref IPositionableObject p)
        {
            a.StorageContainer.Remove(a);
            p.StorageContainer.Remove(p);
            a = null;
            p = null;
        }
    }
}
