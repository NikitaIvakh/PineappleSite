namespace PineappleSite.Presentation.Services.Favorites
{
    public class FavouriteCollectionResult<TEntity> : FavouriteResult<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}