using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Contracts
{
    public interface IFavoriteService
    {
        Task<FavoritesResponseViewModel> GetFavoritesAsync(string userId);

        Task<FavoritesResponseViewModel> FavoritesUpsertAsync(FavouritesViewModel viewModel);

        Task<FavoritesResponseViewModel> DeleteFavoriteDetails(int favoriteId);
    }
}