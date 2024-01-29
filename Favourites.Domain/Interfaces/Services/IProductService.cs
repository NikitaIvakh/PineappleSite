using Favourites.Domain.DTOs;

namespace Favourites.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductDto>> GetProductListAsync();
    }
}