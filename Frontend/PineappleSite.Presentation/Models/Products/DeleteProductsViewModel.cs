namespace PineappleSite.Presentation.Models.Products;

public sealed class DeleteProductsViewModel(IList<int> productIds)
{
    public IList<int> ProductIds { get; set; } = productIds;
}