namespace Favourites.Domain.ResultFavourites
{
    public class CollectionResult<TEntity> : Result<TEntity>
    {
        public int Count { get; set; }
    }
}