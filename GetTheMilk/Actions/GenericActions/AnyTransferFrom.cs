using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions.GenericActions
{
    public abstract class AnyTransferFrom:ObjectTransferAction
    {
        public override bool Perform(ICharacter a, ICharacter p)
        {
            if (UseableObject.StorageContainer.Owner.Name == p.Name)
                return (a.TryAnySuitableInventories(UseableObject));
            return false;
        }
    }
}
