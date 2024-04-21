using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Commands;

public sealed class DeleteFavouriteProductRequestHandler(
    IBaseRepository<FavouriteHeader> headerRepository,
    IBaseRepository<FavouriteDetails> detailsRepository,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductRequest, Result<Unit>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<Unit>> Handle(DeleteFavouriteProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var favouriteProduct =
                detailsRepository.GetAll().FirstOrDefault(key => key.ProductId == request.ProductId);

            if (favouriteProduct is null)
            {
                return new Result<Unit>
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

            var totalRemoveFavouriteItems = detailsRepository
                .GetAll().Count(key => key.FavouriteHeaderId == favouriteProduct.FavouriteHeaderId);

            await detailsRepository.DeleteAsync(favouriteProduct);

            if (totalRemoveFavouriteItems == 1)
            {
                var favouriteHeaderDelete = headerRepository.GetAll()
                    .FirstOrDefault(key => key.FavouriteHeaderId == favouriteProduct.FavouriteHeaderId);

                if (favouriteHeaderDelete is null)
                {
                    return new Result<Unit>
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage =
                            ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound", ErrorMessages.Culture),
                        ValidationErrors =
                        [
                            ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound", ErrorMessages.Culture) ??
                            string.Empty
                        ]
                    };
                }

                await headerRepository.DeleteAsync(favouriteHeaderDelete);
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}