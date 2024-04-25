using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Contracts;

public interface IProductService
{
    Task<ProductsCollectionResultViewModel<GetProductsViewModel>> GetAllProductsAsync();

    Task<ProductResultViewModel<GetProductViewModel>> GetProductAsync(int id);

    Task<ProductResultViewModel<int>> CreateProductAsync(CreateProductViewModel product);

    Task<ProductResultViewModel> UpdateProductAsync(int id, UpdateProductViewModel product);

    Task<ProductResultViewModel> DeleteProductAsync(int id, DeleteProductViewModel product);

    Task<ProductsCollectionResultViewModel<bool>> DeleteProductsAsync(DeleteProductsViewModel product);
}