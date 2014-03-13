using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Accounts
{
    public class TransactionDetails
    {
        public int Price { get; set; }

        public ICharacter Towards { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
