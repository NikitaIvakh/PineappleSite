namespace Favourites.Domain.ResultFavourites
{
    public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}