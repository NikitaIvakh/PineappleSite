using Favourites.Application.DTOs;

namespace Favourites.Application.Services.IServices
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductDto>> GetProductListAsync();
    }
}