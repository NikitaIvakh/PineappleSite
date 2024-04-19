namespace Favourite.Domain.Results
{
    public class CollectionResult<T> : Result<IReadOnlyCollection<T>>
    {
        public int Count { get; init; }
    }
}