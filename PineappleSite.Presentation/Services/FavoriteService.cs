using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Services
{
    public class FavoriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient, IMapper mapper) : BaseFavoriteService(localStorageService, favoritesClient), IFavoriteService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;
        private readonly IMapper _mapper = mapper;

        public async Task<FavoritesResponseViewModel> GetFavoritesAsync(string userId)
        {
            var favorites = await _favoritesClient.GetFavouriteAsync(userId);
            return _mapper.Map<FavoritesResponseViewModel>(favorites);
        }

        public async Task<FavoritesResponseViewModel> FavoritesUpsertAsync(FavouritesViewModel viewModel)
        {
            try
            {
                FavoritesResponseViewModel response = new();
                FavouritesDto favouritesDto = _mapper.Map<FavouritesDto>(viewModel);
                FavouriteAPIResponse apiResponse = await _favoritesClient.FavouriteUpsertAsync(favouritesDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (FavoritesExceptions exception)
            {
                return ConvertFavoriteExceptions(exception);
            }
        }

        public async Task<FavoritesResponseViewModel> DeleteFavoriteDetails(int favoriteId)
        {
            try
            {
                FavoritesResponseViewModel response = new();
                FavouriteAPIResponse apiResponse = await _favoritesClient.RemoveDetailsAsync(favoriteId.ToString(), favoriteId);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Data = apiResponse.Data;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (FavoritesExceptions exception)
            {
                return ConvertFavoriteExceptions(exception);
            }
        }
    }
}