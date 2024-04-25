using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Contracts;

public interface IProductService
{
    Task<ProductsCollectionResultViewModel<ProductViewModel>> GetAllProductsAsync();

    Task<ProductResultViewModel<ProductViewModel>> GetProductAsync(int productId);

    Task<ProductResultViewModel<int>> CreateProductAsync(CreateProductViewModel product);

    Task<ProductResultViewModel> UpdateProductAsync(int productId, UpdateProductViewModel product);

    Task<ProductResultViewModel> DeleteProductAsync(int productId, DeleteProductViewModel product);

    Task<ProductsCollectionResultViewModel<bool>> DeleteProductsAsync(DeleteProductsViewModel product);
}