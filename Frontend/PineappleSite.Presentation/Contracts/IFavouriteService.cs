using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favourite;

namespace PineappleSite.Presentation.Contracts;

public interface IFavouriteService
{
    Task<FavouriteResult<FavouriteViewModel>> GetFavouriteProductsAsync(string userId);

    Task<FavouriteResult<FavouriteHeaderViewModel>> FavouriteProductUpsertAsync(FavouriteViewModel favouriteViewModel);

    Task<FavouriteResult> DeleteFavouriteProductAsync(DeleteProductViewModel deleteProductViewModel);

    Task<FavouriteResult> DeleteFavouriteProductByUserAsync(
        DeleteFavouriteProductByUserViewModel deleteFavouriteProductByUserViewModel);

    Task<FavouriteCollectionResult> DeleteFavouriteProductsAsync(DeleteProductsViewModel deleteProductsViewModel);
}