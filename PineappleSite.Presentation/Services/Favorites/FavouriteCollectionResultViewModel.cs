namespace PineappleSite.Presentation.Services.Favorites
{
    public class FavouriteCollectionResultViewModel<TEntity> : FavouriteResultViewModel<TEntity> where TEntity : class
    {
        public int Count { get; set; }
    }
}