namespace GetTheMilk.Objects.BaseObjects
{
    public class Tool : NonCharacterObject, ITransactionalObject
    {
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }

        public Tool()
        {
            ObjectCategory = ObjectCategory.Tool;
        }
    }
}
