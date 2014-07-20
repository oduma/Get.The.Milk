using System;
using Newtonsoft.Json;

namespace GetTheMilk.Characters.BaseCharacters
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

        public void PerformTransaction(ICharacter towards, int price)
        {
            CurrentCapacity -=price;
            towards.Walet.CurrentCapacity += price;
        }

        public bool CanPerformTransaction(ICharacter towards, int price)
        {
            return (CurrentCapacity > price) &&
                    (towards.Walet.MaxCapacity >=
                    towards.Walet.CurrentCapacity + price);
        }
    }
}
