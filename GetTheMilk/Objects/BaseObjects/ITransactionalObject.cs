namespace GetTheMilk.Objects.BaseObjects
{
    public interface ITransactionalObject : INonCharacterObject
    {
        int BuyPrice { get; }

        int SellPrice { get; }
    }
}
