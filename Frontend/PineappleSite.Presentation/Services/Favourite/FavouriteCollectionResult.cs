namespace PineappleSite.Presentation.Services.Favourite;

public sealed class FavouriteCollectionResult<T> : FavouriteResult<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}