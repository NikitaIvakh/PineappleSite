namespace PineappleSite.Presentation.Services.Favourite;

public sealed class FavouriteCollectionResult : FavouriteResult
{
    public int Count { get; set; }
}

public sealed class FavouriteCollectionResult<T> : FavouriteResult<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}