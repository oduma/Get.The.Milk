using GetTheMilk.Accounts;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Actions
{
    public class TakeMoneyFrom:GameAction
    {
        public TakeMoneyFrom()
        {
            Name = new Verb {Infinitive = "To Take Money", Past = "took money", Present = "take money"};
            ActionType = ActionType.TakeMoneyFrom;
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
