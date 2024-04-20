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

public sealed class DeleteFavouriteProductsRequestHandler(
    IBaseRepository<FavouriteHeader> favouriteHeaderRepository,
    IBaseRepository<FavouriteDetails> favouriteDetailsRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductsRequest, Result<FavouriteHeaderDto>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<FavouriteHeaderDto>> Handle(DeleteFavouriteProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var favouriteDetails = favouriteDetailsRepository.GetAll()
                .Where(key => request.DeleteFavourite.ProductIds.Contains(key.ProductId)).ToList();

            if (favouriteDetails.Count == 0)
            {
                return new Result<FavouriteHeaderDto>
                {
                    Data = new FavouriteHeaderDto(),
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }
            
            var favouriteHeader = favouriteHeaderRepository.GetAll().FirstOrDefault(key =>
                key.FavouriteHeaderId == favouriteDetails.FirstOrDefault()!.FavouriteHeaderId);

            var totalRemoveProducts = favouriteDetailsRepository.GetAll().Count(key =>
                key.FavouriteHeaderId == favouriteDetails.FirstOrDefault()!.FavouriteHeaderId);

            foreach (var favouriteDetail in favouriteDetails)
            {
                await favouriteDetailsRepository.DeleteAsync(favouriteDetail);
            }

            if (totalRemoveProducts == 1)
            {
                var favouriteHeaderFromDb = favouriteHeaderRepository.GetAll().FirstOrDefault(key =>
                    key.FavouriteHeaderId == favouriteDetails.FirstOrDefault()!.FavouriteHeaderId);

                if (favouriteHeaderFromDb is not null)
                {
                    await favouriteHeaderRepository.DeleteAsync(favouriteHeaderFromDb);
                }
            }

            var getAllHeaders = favouriteHeaderRepository.GetAll().ToList();
            var getAllDetails = favouriteDetailsRepository.GetAll().ToList();

            memoryCache.Remove(getAllHeaders);
            memoryCache.Remove(getAllDetails);
            memoryCache.Set(CacheKey, getAllHeaders);
            memoryCache.Set(CacheKey, getAllDetails);

            return new Result<FavouriteHeaderDto>
            {
                StatusCode = (int)StatusCode.Deleted,
                Data = mapper.Map<FavouriteHeaderDto>(favouriteHeader),
                SuccessMessage = SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyDeleted", SuccessMessage.Culture),
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