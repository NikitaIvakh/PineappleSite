using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;
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
                        SuccessCode = favouriteProducts.SuccessCode,
                        SuccessMessage = favouriteProducts.SuccessMessage,
                        Data = _mapper.Map<FavouriteViewModel>(favouriteProducts.Data),
                    };

                    return result;
                }

                else
                {
                    foreach (var error in favouriteProducts.ValidationErrors)
                    {
                        return new FavouriteResult<FavouriteViewModel>
                        {
                            ValidationErrors = [error],
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
                    ValidationErrors = [exceptions.Response]
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
                        SuccessCode = apiResponse.SuccessCode,
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
                            ValidationErrors = [error],
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
                    ValidationErrors = [exceptions.Response]
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
                        SuccessCode = apiResponse.SuccessCode,
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
                            ValidationErrors = [error],
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
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<FavouriteResult<FavouriteViewModel>> FavouruteRemoveProductsListAsync(DeleteProductsViewModel deleteProductsViewModel)
        {
            try
            {
                var favouriteDto = _mapper.Map<DeleteFavouriteProducts>(deleteProductsViewModel);
                var apiResult = await _favoritesClient.DeleteProductListAsync(favouriteDto);

                if (apiResult.IsSuccess)
                {
                    return new FavouriteResult<FavouriteViewModel>
                    {
                        SuccessCode = apiResult.SuccessCode,
                        SuccessMessage = apiResult.SuccessMessage,
                        Data = _mapper.Map<FavouriteViewModel>(apiResult.Data),
                    };
                }

                else
                {
                    foreach (var error in apiResult.ValidationErrors)
                    {
                        return new FavouriteResult<FavouriteViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResult.ErrorCode,
                            ErrorMessage = apiResult.ErrorMessage,
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
                    ValidationErrors = [exceptions.Response]
                };
            }
        }
    }
}