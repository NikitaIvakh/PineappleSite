using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Contracts
{
    public interface IProductService
    {
        Task<ProductsCollectionResultViewModel<ProductViewModel>> GetAllProductsAsync();

        Task<ProductResultViewModel<ProductViewModel>> GetProductAsync(int id);

        Task<ProductResultViewModel<ProductViewModel>> CreateProductAsync(CreateProductViewModel product);

        Task<ProductResultViewModel<ProductViewModel>> UpdateProductAsync(int id, UpdateProductViewModel product);

        Task<ProductResultViewModel<ProductViewModel>> DeleteProductAsync(int id, DeleteProductViewModel product);

        Task<ProductsCollectionResultViewModel<ProductViewModel>> DeleteProductsAsync(DeleteProductsViewModel product);
    }
}