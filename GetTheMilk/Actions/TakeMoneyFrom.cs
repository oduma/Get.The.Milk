using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class TakeMoneyFrom:GameAction
    {
        public override string Name
        {
            get { return "TakeMoneyFrom"; }
        }

        public int Amount { get; set; }
        public void Perform(ICharacter fromCharacter, ICharacter toCharacter)
        {
            fromCharacter.Walet.PerformTransaction(new TransactionDetails
                                                       {
                                                           Price = Amount,
                                                           Towards=toCharacter,
                                                           TransactionType = TransactionType.Debit
                                                       });
        }
    }
}
