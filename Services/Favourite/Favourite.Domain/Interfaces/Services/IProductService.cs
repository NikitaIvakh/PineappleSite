using Favourite.Domain.DTOs;
using Favourite.Domain.Results;

namespace Favourite.Domain.Interfaces.Services;

public interface IProductService
{
    Task<CollectionResult<ProductDto>> GetProductListAsync();
}