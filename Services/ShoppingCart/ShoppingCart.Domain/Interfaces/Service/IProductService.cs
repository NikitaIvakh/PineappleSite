using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Domain.Interfaces.Service
{
    public interface IProductService
    {
        Task<CollectionResult<ProductDto>> GetProductListAsync();
    }
}