namespace PineappleSite.Presentation.Services.Orders
{
    public class OrderCollectionResult<TEntity> : OrderResult<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}