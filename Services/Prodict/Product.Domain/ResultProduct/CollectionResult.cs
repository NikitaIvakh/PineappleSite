namespace Product.Domain.ResultProduct;

public sealed class CollectionResult<T> : Result<IReadOnlyCollection<T>>
{
    public int Count { get; init; }
}