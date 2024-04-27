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

public sealed class DeleteFavouriteProductsRequestHandler(
    IBaseRepository<FavouriteHeader> favouriteHeaderRepository,
    IBaseRepository<FavouriteDetails> favouriteDetailsRepository,
    DeleteProductsValidator deleteProductsValidator,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductsRequest, CollectionResult<Unit>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<CollectionResult<Unit>> Handle(DeleteFavouriteProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult =
                await deleteProductsValidator.ValidateAsync(request.DeleteFavourite, cancellationToken);

            if (!validationResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "ProductIds", validationResult.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new CollectionResult<Unit>()
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture)
                        };
                    }
                }

                return new CollectionResult<Unit>()
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture),
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList(),
                };
            }

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

            var favouriteHeaderIds = favouriteDetails.Select(fd => fd.FavouriteHeaderId).Distinct().ToList();
            var favouriteDetailsToDelete = favouriteDetails
                .Where(fd => request.DeleteFavourite.ProductIds.Contains(fd.ProductId)).ToList();

            foreach (var favouriteDetail in favouriteDetailsToDelete)
            {
                await favouriteDetailsRepository.DeleteAsync(favouriteDetail);
            }

            foreach (var favouriteHeaderFromDb in from favouriteHeaderId in favouriteHeaderIds
                     let totalDetailsWithHeader = favouriteDetailsRepository.GetAll()
                         .Count(key => key.FavouriteHeaderId == favouriteHeaderId)
                     where totalDetailsWithHeader == 0
                     select favouriteHeaderRepository.GetAll()
                         .FirstOrDefault(key => key.FavouriteHeaderId == favouriteHeaderId))
            {
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
                Data = new List<Unit> { Unit.Value },
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