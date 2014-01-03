using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public abstract class ObjectUseOnObjectAction : GameAction
    {
        public abstract void Perform(ref IPositionableObject uObject, ref IPositionableObject pObject);
    }
}
