using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Application.Validators;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Commands;

public sealed class DeleteFavouriteProductByUserRequestHandler(
    IBaseRepository<FavouriteHeader> favouriteHeaderRepository,
    IBaseRepository<FavouriteDetails> favouriteDetailsRepository,
    DeleteByUserValidator deleteByUserValidator,
    IMemoryCache memoryCache)
    : IRequestHandler<DeleteFavouriteProductByUserRequest, Result<Unit>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<Unit>> Handle(DeleteFavouriteProductByUserRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult =
                await deleteByUserValidator.ValidateAsync(request.DeleteFavouriteProductByUserDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "ProductId", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "UserId", validationResult.Errors.Select(key => key.ErrorMessage).ToList() },
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>()
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NotFound,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                        };
                    }
                }

                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList()
                };
            }

            var favouriteProduct = favouriteDetailsRepository.GetAll().FirstOrDefault(key =>
                key.ProductId == request.DeleteFavouriteProductByUserDto.ProductId &&
                key.FavouriteHeader!.UserId == request.DeleteFavouriteProductByUserDto.UserId);

            if (favouriteProduct is null)
            {
                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("ProductNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }
            
            var productCount = favouriteDetailsRepository.GetAll()
                .Count(key => key.FavouriteHeaderId == favouriteProduct.FavouriteHeaderId);

            switch (productCount)
            {
                case 1:
                {
                    var favouriteHeader = favouriteHeaderRepository.GetAll().FirstOrDefault(key =>
                        key.FavouriteHeaderId == favouriteProduct.FavouriteHeaderId &&
                        key.UserId == request.DeleteFavouriteProductByUserDto.UserId);

                    if (favouriteHeader is null)
                    {
                        return new Result<Unit>()
                        {
                            StatusCode = (int)StatusCode.NotFound,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound",
                                    ErrorMessages.Culture),
                            ValidationErrors =
                            [
                                ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound",
                                    ErrorMessages.Culture) ??
                                string.Empty
                            ]
                        };
                    }

                    await favouriteDetailsRepository.DeleteAsync(favouriteProduct);
                    await favouriteHeaderRepository.DeleteAsync(favouriteHeader);
                    break;
                }

                case > 1:
                    await favouriteDetailsRepository.DeleteAsync(favouriteProduct);
                    break;
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>()
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>()
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}