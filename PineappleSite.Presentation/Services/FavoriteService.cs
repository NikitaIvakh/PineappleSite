using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Services.Favorites;

namespace PineappleSite.Presentation.Services
{
    public class FavoriteService(ILocalStorageService localStorageService, IFavoritesClient favoritesClient, IMapper mapper) : BaseHttpFavouriteService(localStorageService, favoritesClient), IFavoriteService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IFavoritesClient _favoritesClient = favoritesClient;
        private readonly IMapper _mapper = mapper;

        public async Task<FavouriteResult<FavouriteViewModel>> GetFavouruteProductsAsync(string userId)
        {
            AddBearerToken();
            try
            {
                FavouriteDtoResult favouriteProducts = await _favoritesClient.GetFavouriteProductsAsync(userId);

                if (favouriteProducts.IsSuccess)
                {
                    FavouriteResult<FavouriteViewModel> result = new()
                    {
                        Data = _mapper.Map<FavouriteViewModel>(favouriteProducts.Data),
                        ErrorCode = favouriteProducts.ErrorCode,
                        ErrorMessage = favouriteProducts.ErrorMessage,
                        SuccessMessage = favouriteProducts.SuccessMessage,
                    };

                    return result;
                }

                else
                {
                    foreach (var error in favouriteProducts.ValidationErrors)
                    {
                        return new FavouriteResult<FavouriteViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = favouriteProducts.ErrorCode,
                            ErrorMessage = favouriteProducts.ErrorMessage,
                        };
                    }
                }

                return new FavouriteResult<FavouriteViewModel>();
            }

            catch (FavoritesExceptions exceptions)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                };
            }
        }

        public async Task<FavouriteResult<FavouriteViewModel>> FavouruteUpsertProductsAsync(FavouriteViewModel favouriteViewModel)
        {
            AddBearerToken();
            try
            {
                FavouriteDto favouriteDto = _mapper.Map<FavouriteDto>(favouriteViewModel);
                FavouriteDtoResult apiResponse = await _favoritesClient.FavouriteUpsertAsync(favouriteDto);

                if (apiResponse.IsSuccess)
                {
                    return new FavouriteResult<FavouriteViewModel>
                    {
                        Data = _mapper.Map<FavouriteViewModel>(apiResponse.Data),
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new FavouriteResult<FavouriteViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new FavouriteResult<FavouriteViewModel>();
            }

            catch (FavoritesExceptions exceptions)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            };
        }

        public async Task<FavouriteResult<FavouriteViewModel>> FavouruteRemoveProductsAsync(int productUd)
        {
            AddBearerToken();
            try
            {
                FavouriteDtoResult apiResponse = await _favoritesClient.FavouriteAsync(productUd);

                if (apiResponse.IsSuccess)
                {
                    return new FavouriteResult<FavouriteViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<FavouriteViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResponse.ValidationErrors)
                    {
                        return new FavouriteResult<FavouriteViewModel>
                        {
                            ValidationErrors = error,
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new FavouriteResult<FavouriteViewModel>();
            }

            catch (FavoritesExceptions exceptions)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
            }
        }
    }
}