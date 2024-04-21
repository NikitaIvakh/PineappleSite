using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Commands;

public sealed class DeleteFavouriteProductsRequestHandler(
    IBaseRepository<FavouriteHeader> favouriteHeaderRepository,
    IBaseRepository<FavouriteDetails> favouriteDetailsRepository,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductsRequest, CollectionResult<Unit>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<CollectionResult<Unit>> Handle(DeleteFavouriteProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var favouriteDetails = favouriteDetailsRepository.GetAll()
                .Where(key => request.DeleteFavourite.ProductIds.Contains(key.ProductId)).ToList();

            if (favouriteDetails.Count == 0)
            {
                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            foreach (var favouriteDetail in favouriteDetails)
            {
                await favouriteDetailsRepository.DeleteAsync(favouriteDetail);
            }

            var totalRemoveProducts = favouriteDetailsRepository.GetAll().Count(key =>
                key.FavouriteHeaderId == favouriteDetails.FirstOrDefault()!.FavouriteHeaderId);

            if (totalRemoveProducts == 1)
            {
                var favouriteHeaderFromDb = favouriteHeaderRepository.GetAll().FirstOrDefault(key =>
                    key.FavouriteHeaderId == favouriteDetails.FirstOrDefault()!.FavouriteHeaderId);

                if (favouriteHeaderFromDb is null)
                {
                    return new CollectionResult<Unit>()
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

                await favouriteHeaderRepository.DeleteAsync(favouriteHeaderFromDb);
            }

            memoryCache.Remove(CacheKey);

            return new CollectionResult<Unit>
            {
                Data = [Unit.Value],
                Count = favouriteDetails.Count,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}