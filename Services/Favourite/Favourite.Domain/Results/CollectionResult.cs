namespace Favourite.Domain.Results
{
    public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}