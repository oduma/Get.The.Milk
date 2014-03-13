using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class TakeFrom : ObjectTransferAction
    {
        public TakeFrom()
        {
            Name = new Verb { Infinitive = "To Give", Past = "gave", Present = "give" };
            TransactionType = TransactionType.None;
            ActionType = ActionType.TakeFrom;
        }
        public override bool Perform(ICharacter a, ICharacter p)
        {
            return base.Perform(p, a);
        }

    }
}
