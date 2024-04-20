using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Favourites;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Favourite;

namespace PineappleSite.Presentation.Services;

public sealed class FavouriteService(
    ILocalStorageService localStorageService,
    IFavouriteClient favouriteClient,
    IMapper mapper,
    IHttpContextAccessor contextAccessor)
    : BaseHttpFavouriteService(localStorageService, favouriteClient, contextAccessor), IFavouriteService
{
    private readonly IFavouriteClient _favouriteClient = favouriteClient;

    public async Task<FavouriteResult<FavouriteViewModel>> GetFavouriteProductsAsync(string userId)
    {
        AddBearerToken();
        try
        {
            var favouriteProducts = await _favouriteClient.GetFavouriteProductsAsync(userId);

            if (!favouriteProducts.IsSuccess)
            {
                return new FavouriteResult<FavouriteViewModel>
                {
                    StatusCode = favouriteProducts.StatusCode,
                    ErrorMessage = favouriteProducts.ErrorMessage,
                    ValidationErrors = string.Join(", ", favouriteProducts.ValidationErrors),
                };
            }

            FavouriteResult<FavouriteViewModel> result = new()
            {
                StatusCode = favouriteProducts.StatusCode,
                SuccessMessage = favouriteProducts.SuccessMessage,
                Data = mapper.Map<FavouriteViewModel>(favouriteProducts.Data),
            };

            return result;
        }

        catch (FavouriteExceptions<string> ex)
        {
            return new FavouriteResult<FavouriteViewModel>
            {
                StatusCode = ex.StatusCode,
                ErrorMessage = ex.Response,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }

    public async Task<FavouriteResult<FavouriteHeaderViewModel>> FavouriteProductUpsertAsync(
        FavouriteViewModel favouriteViewModel)
    {
        AddBearerToken();
        try
        {
            var favouriteDto = mapper.Map<FavouriteDto>(favouriteViewModel);
            var apiResponse = await _favouriteClient.FavouriteUpsertAsync(favouriteDto);

            if (apiResponse.IsSuccess)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<FavouriteHeaderViewModel>(apiResponse.Data),
                };
            }

            if (apiResponse.ValidationErrors.Count != 0)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    ErrorMessage = apiResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
                };
            }

            return new FavouriteResult<FavouriteHeaderViewModel>();
        }

        catch (FavouriteExceptions<string> exceptions)
        {
            return new FavouriteResult<FavouriteHeaderViewModel>
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Response,
                ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
            };
        }
    }

    public async Task<FavouriteResult<FavouriteHeaderViewModel>> DeleteFavouriteProductAsync(int productId)
    {
        AddBearerToken();
        try
        {
            var apiResponse = await _favouriteClient.DeleteFavouriteProductAsync(productId);

            if (apiResponse.IsSuccess)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<FavouriteHeaderViewModel>(apiResponse.Data),
                };
            }

            if (apiResponse.ValidationErrors.Count != 0)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    ErrorMessage = apiResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
                };
            }

            return new FavouriteResult<FavouriteHeaderViewModel>();
        }

        catch (FavouriteExceptions<string> exceptions)
        {
            return new FavouriteResult<FavouriteHeaderViewModel>
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Response,
                ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
            };
        }
    }

    public async Task<FavouriteResult<FavouriteHeaderViewModel>> DeleteFavouriteProductsAsync(
        DeleteProductsViewModel deleteProductsViewModel)
    {
        AddBearerToken();
        try
        {
            var favouriteDto = mapper.Map<DeleteFavouriteProductsDto>(deleteProductsViewModel);
            var apiResponse = await _favouriteClient.DeleteFavouriteProductsAsync(favouriteDto);

            if (apiResponse.IsSuccess)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<FavouriteHeaderViewModel>(apiResponse.Data),
                };
            }

            if (apiResponse.ValidationErrors.Count != 0)
            {
                return new FavouriteResult<FavouriteHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    ErrorMessage = apiResponse.ErrorMessage,
                    ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
                };
            }

            return new FavouriteResult<FavouriteHeaderViewModel>();
        }

        catch (FavouriteExceptions<string> exceptions)
        {
            return new FavouriteResult<FavouriteHeaderViewModel>
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Response,
                ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
            };
        }
    }
}