using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Contracts
{
    public interface IFavoriteService
    {
        Task<FavouriteResultViewModel<FavouritesViewModel>> GetFavoritesAsync(string userId);

        Task<FavouriteResultViewModel<FavouritesHeaderViewModel>> FavoritesUpsertAsync(FavouritesViewModel viewModel);

        Task<FavouriteResultViewModel<FavouritesViewModel>> DeleteFavoriteDetails(int favoriteId);
    }
}