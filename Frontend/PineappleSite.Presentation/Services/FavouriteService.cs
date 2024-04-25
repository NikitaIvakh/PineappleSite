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

            return new FavouriteResult<FavouriteHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
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

    public async Task<FavouriteResult> DeleteFavouriteProductAsync(int productId)
    {
        AddBearerToken();
        try
        {
            var apiResponse = await _favouriteClient.DeleteFavouriteProductAsync(productId);

            if (apiResponse.IsSuccess)
            {
                return new FavouriteResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new FavouriteResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (FavouriteExceptions<string> exceptions)
        {
            return new FavouriteResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Response,
                ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
            };
        }
    }

    public async Task<FavouriteCollectionResult> DeleteFavouriteProductsAsync(
        DeleteProductsViewModel deleteProductsViewModel)
    {
        AddBearerToken();
        try
        {
            var favouriteDto = mapper.Map<DeleteFavouriteProductsDto>(deleteProductsViewModel);
            var apiResponse = await _favouriteClient.DeleteFavouriteProductsAsync(favouriteDto);

            if (apiResponse.IsSuccess)
            {
                return new FavouriteCollectionResult
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new FavouriteCollectionResult
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (FavouriteExceptions<string> exceptions)
        {
            return new FavouriteCollectionResult
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Response,
                ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
            };
        }
    }
}