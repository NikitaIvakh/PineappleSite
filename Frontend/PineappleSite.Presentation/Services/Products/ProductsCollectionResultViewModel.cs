namespace PineappleSite.Presentation.Services.Products
{
    public class ProductsCollectionResultViewModel<Type> : ProductResultViewModel<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}