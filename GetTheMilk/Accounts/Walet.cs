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
            if (transactionDetails.TransactionType == TransactionType.None)
                return false;

            CurrentCapacity -=transactionDetails.Price;
            transactionDetails.Towards.Walet.CurrentCapacity += transactionDetails.Price;
            return true;
        }

        public bool CanPerformTransaction(TransactionDetails transactionDetails)
        {
            //free gift
            if (transactionDetails.TransactionType == TransactionType.None)
                return true;
            //give money
            if(transactionDetails.TransactionType==TransactionType.Payed)
            {
                return (CurrentCapacity > transactionDetails.Price) &&
                       (transactionDetails.Towards.Walet.MaxCapacity >=
                        transactionDetails.Towards.Walet.CurrentCapacity + transactionDetails.Price);
            }
            return false;
        }
    }
}
