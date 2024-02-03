using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favorites;
using PineappleSite.Presentation.Services.Coupons;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Services
{
    public class FavoriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient, IMapper mapper) : BaseFavoriteService(localStorageService, favoritesClient), IFavoriteService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;
        private readonly IMapper _mapper = mapper;

        public async Task<FavouriteResultViewModel<FavouritesViewModel>> GetFavoritesAsync(string userId)
        {
            AddBearerToken();
            FavouritesDtoResult favorites = await _favoritesClient.GetFavouriteAsync(userId);
            FavouriteResultViewModel<FavouritesViewModel> resultViewModel = new()
            {
                Data = _mapper.Map<FavouritesViewModel>(favorites.Data),
                ErrorMessage = favorites.ErrorMessage,
                ErrorCode = favorites.ErrorCode,
                SuccessMessage = favorites.SuccessMessage,
            };

            return resultViewModel;
        }

        public async Task<FavouriteResultViewModel<FavouritesViewModel>> FavoritesUpsertAsync(FavouritesViewModel viewModel)
        {
            try
            {
                FavouritesDto favouritesDto = _mapper.Map<FavouritesDto>(viewModel);
                FavouritesDtoResult apiResponse = await _favoritesClient.FavouriteUpsertAsync(favouritesDto);

                if (apiResponse.IsSuccess)
                {
                    return new FavouriteResultViewModel<FavouritesViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<FavouritesViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new FavouriteResultViewModel<FavouritesViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new FavouriteResultViewModel<FavouritesViewModel>();
            }

            catch (FavoritesExceptions exception)
            {
                return new FavouriteResultViewModel<FavouritesViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                    ValidationErrors = exception.Message,
                };
            }
        }

        public async Task<FavouriteResultViewModel<FavouritesViewModel>> DeleteFavoriteDetails(int favoriteId)
        {
            try
            {
                FavouritesDtoResult apiResponse = await _favoritesClient.RemoveFavouriteDetailsAsync(favoriteId.ToString(), favoriteId);

                if (apiResponse.IsSuccess)
                {
                    return new FavouriteResultViewModel<FavouritesViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<FavouritesViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new FavouriteResultViewModel<FavouritesViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new FavouriteResultViewModel<FavouritesViewModel>();
            }

            catch (FavoritesExceptions exception)
            {
                return new FavouriteResultViewModel<FavouritesViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                    ValidationErrors = exception.Message,
                };
            }
        }
    }
}