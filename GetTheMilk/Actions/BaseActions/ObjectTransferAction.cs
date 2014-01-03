using GetTheMilk.Accounts;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions.BaseActions
{
    public abstract class ObjectTransferAction : GameAction
    {
        public abstract TransactionType TransactionType { get;}

        public abstract bool Perform(ICharacter active, ICharacter passive);

        public IPositionableObject UseableObject { get; set; }
    }
}
