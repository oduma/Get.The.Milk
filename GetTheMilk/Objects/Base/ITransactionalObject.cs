namespace GetTheMilk.Objects.Base
{
    public interface ITransactionalObject : INonCharacterObject
    {
        int BuyPrice { get; }

        int SellPrice { get; }
    }
}
