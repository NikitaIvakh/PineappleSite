using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Contracts
{
    public interface IFavoriteService
    {
        Task<FavouriteResult<FavouriteViewModel>> GetFavouruteProductsAsync(string userId);

        Task<FavouriteResult<FavouriteViewModel>> FavouruteUpsertProductsAsync(FavouriteViewModel favouriteViewModel);

        Task<FavouriteResult<FavouriteViewModel>> FavouruteRemoveProductsAsync(int productUd);

        Task<FavouriteResult<FavouriteViewModel>> FavouruteRemoveProductsListAsync(DeleteProductsViewModel deleteProductsViewModel);
    }
}