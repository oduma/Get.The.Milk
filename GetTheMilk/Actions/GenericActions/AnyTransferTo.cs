using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.GenericActions
{
    public abstract class AnyTransferTo:ObjectTransferAction
    {
        public override bool Perform(ICharacter a, ICharacter p)
        {
            if (UseableObject.StorageContainer.Owner.Name == a.Name)
                return (p.TryAnySuitableInventories(UseableObject));
            return false;
        }
    }
}
