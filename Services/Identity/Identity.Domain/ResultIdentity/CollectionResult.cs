namespace Identity.Domain.ResultIdentity;

public class CollectionResult<T> : Result<IReadOnlyCollection<T>>
{
    public int Count { get; init; }
}