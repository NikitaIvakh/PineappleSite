using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<CollectionResult<ProductDto>> GetProductListAsync();
    }
}