using GetTheMilk.Accounts;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public class ObjectTransferAction : GameAction
    {
        public TransactionType TransactionType { get; protected set; }

        public NonCharacterObject UseableObject { get; set; }

        public virtual bool Perform(ICharacter active, ICharacter passive)
        {
            if (UseableObject.StorageContainer.Owner.Name == active.Name)
                return (passive.Inventory.Add(UseableObject));
            return false;

        }
    }
}
