using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IProductService
    {
        Task<CollectionResult<ProductDto>> GetProductsAsync();
    }
}