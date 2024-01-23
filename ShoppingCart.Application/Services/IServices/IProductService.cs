using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}