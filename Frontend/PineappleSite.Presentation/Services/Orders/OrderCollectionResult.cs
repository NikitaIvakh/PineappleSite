namespace PineappleSite.Presentation.Services.Orders;

public sealed class OrderCollectionResult<T> : OrderResult<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}