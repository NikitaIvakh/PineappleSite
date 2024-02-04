namespace ShoppingCart.Domain.ResultCart
{
    public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}