using GetTheMilk.Common;

namespace GetTheMilk.Objects.Base
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
