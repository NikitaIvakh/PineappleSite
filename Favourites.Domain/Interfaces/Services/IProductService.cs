using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;

namespace Favourites.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<CollectionResult<ProductDto>> GetProductListAsync();
    }
}