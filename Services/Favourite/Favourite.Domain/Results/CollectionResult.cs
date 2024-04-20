namespace Favourite.Domain.Results;

public sealed class CollectionResult<T> : Result<IReadOnlyCollection<T>>
{
    public int Count { get; init; }
}