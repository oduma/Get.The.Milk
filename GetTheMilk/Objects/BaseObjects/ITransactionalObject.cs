namespace GetTheMilk.Objects.BaseObjects
{
    public interface ITransactionalObject : IPositionableObject
    {
        int BuyPrice { get; }

        int SellPrice { get; }
    }
}
