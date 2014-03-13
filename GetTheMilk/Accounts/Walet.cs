using System;
using GetTheMilk.Characters.BaseCharacters;
using Newtonsoft.Json;

namespace GetTheMilk.Accounts
{
    public class Walet
    {
        private int _currentCapacity;
        public int MaxCapacity { get; set; }

        public int CurrentCapacity
        {
            get { return _currentCapacity; }
            set
            {
                if (_currentCapacity > MaxCapacity)
                    throw new Exception("Not enough space in the wallet.");
                _currentCapacity = value;
            }
        }

        [JsonIgnore]
        public ICharacter Owner { get; set; }


        public bool PerformTransaction(TransactionDetails transactionDetails)
        {
            //give money
            if(transactionDetails.TransactionType==TransactionType.Debit)
            {
                CurrentCapacity -=transactionDetails.Price;
                transactionDetails.Towards.Walet.CurrentCapacity += transactionDetails.Price;
                return true;
            }
            //receive money
            return transactionDetails.Towards.Walet.PerformTransaction(new TransactionDetails
                                                                           {
                                                                               Towards = Owner,
                                                                               TransactionType = TransactionType.Debit,
                                                                               Price = transactionDetails.Price
                                                                           });
        }

        public bool CanPerformTransaction(TransactionDetails transactionDetails)
        {
            //give money
            if(transactionDetails.TransactionType==TransactionType.Debit)
            {
                return (CurrentCapacity > transactionDetails.Price) &&
                       (transactionDetails.Towards.Walet.MaxCapacity >=
                        transactionDetails.Towards.Walet.CurrentCapacity + transactionDetails.Price);
            }
            //receive money
            return
                transactionDetails.Towards.Walet.CanPerformTransaction(new TransactionDetails
                                                                           {
                                                                               TransactionType = TransactionType.Debit,
                                                                               Price = transactionDetails.Price,
                                                                               Towards=Owner
                                                                           });
        }
    }
}
