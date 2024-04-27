namespace Order.Domain.ResultOrder;

public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>>
{
    public int Count { get; init; }
}