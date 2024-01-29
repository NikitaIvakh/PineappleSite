using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Contracts
{
    public interface IFavoriteService
    {
        Task<FavouritesViewModel> GetFavoritesAsync(string userId);

        Task<FavouriteResultViewModel<FavouritesViewModel>> FavoritesUpsertAsync(FavouritesViewModel viewModel);

        Task<FavouriteResultViewModel<FavouritesViewModel>> DeleteFavoriteDetails(int favoriteId);
    }
}