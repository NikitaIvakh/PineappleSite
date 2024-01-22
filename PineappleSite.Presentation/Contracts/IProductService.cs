using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Contracts
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductViewModel>> GetAllProductsAsync();

        Task<ProductViewModel> GetProductAsync(int id);

        Task<ProductAPIViewModel> CreateProductAsync(CreateProductViewModel product);

        Task<ProductAPIViewModel> UpdateProductAsync(int id, UpdateProductViewModel product);

        Task<ProductAPIViewModel> DeleteProductAsync(int id, DeleteProductViewModel product);

        Task<ProductAPIViewModel> DeleteProductsAsync(DeleteProductsViewModel product);
    }
}