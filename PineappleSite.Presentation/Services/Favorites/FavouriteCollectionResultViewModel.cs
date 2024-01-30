namespace PineappleSite.Presentation.Services.Favorites
{
    public class FavouriteCollectionResultViewModel<TEntity> : FavouriteResultViewModel<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}