namespace PineappleSite.Presentation.Services.Products;

public sealed class ProductsCollectionResultViewModel<T> : ProductResultViewModel<IReadOnlyCollection<T>>
{
    public int Count { get; init; }
}