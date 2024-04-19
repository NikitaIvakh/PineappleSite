using AutoMapper;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Domain.DTOs;
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
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductRequest, Result<FavouriteHeaderDto>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<FavouriteHeaderDto>> Handle(DeleteFavouriteProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var favouriteProduct =
                detailsRepository.GetAll().FirstOrDefault(key => key.ProductId == request.ProductId);

            var favouriteHeader = headerRepository.GetAll()
                .FirstOrDefault(key => key.FavouriteHeaderId == favouriteProduct!.FavouriteHeaderId);

            if (favouriteProduct is null)
            {
                return new Result<FavouriteHeaderDto>
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
                    return new Result<FavouriteHeaderDto>
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

            var getAllHeaders = headerRepository.GetAll().ToList();
            var getAllDetails = detailsRepository.GetAll().ToList();

            memoryCache.Remove(getAllHeaders);
            memoryCache.Remove(getAllDetails);
            memoryCache.Set(CacheKey, getAllHeaders);
            memoryCache.Set(CacheKey, getAllDetails);

            return new Result<FavouriteHeaderDto>
            {
                StatusCode = (int)StatusCode.Deleted,
                Data = mapper.Map<FavouriteHeaderDto>(favouriteHeader),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<FavouriteHeaderDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}