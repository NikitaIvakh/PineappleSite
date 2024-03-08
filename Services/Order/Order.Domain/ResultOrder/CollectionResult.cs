namespace Order.Domain.ResultOrder
{
    public class CollectionResult 
    {
        public int Count { get; set; }
    }

    public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}