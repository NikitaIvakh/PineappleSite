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

public sealed class FavouriteProductUpsertRequestHandler(
    IBaseRepository<FavouriteHeader> headerRepository,
    IBaseRepository<FavouriteDetails> detailsRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<FavouriteProductUpsertRequest, Result<FavouriteHeaderDto>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<FavouriteHeaderDto>> Handle(FavouriteProductUpsertRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var favouriteHeaderFromDb = headerRepository.GetAll()
                .FirstOrDefault(key => key.UserId == request.FavouriteDto.FavouriteHeader.UserId);

            if (favouriteHeaderFromDb is null)
            {
                var favouriteHeader =
                    mapper.Map<FavouriteHeader>(request.FavouriteDto.FavouriteHeader);
                await headerRepository.CreateAsync(favouriteHeader!);

                request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId = favouriteHeader!.FavouriteHeaderId;
                await detailsRepository.CreateAsync(
                    mapper.Map<FavouriteDetails>(request.FavouriteDto.FavouriteDetails.First())!);

                var getAllHeaders = headerRepository.GetAll().ToList();
                var getAllDetails = detailsRepository.GetAll().ToList();

                memoryCache.Remove(getAllHeaders);
                memoryCache.Remove(getAllDetails);
                memoryCache.Set(CacheKey, getAllHeaders);
                memoryCache.Set(CacheKey, getAllDetails);

                return new Result<FavouriteHeaderDto>
                {
                    StatusCode = (int)StatusCode.Created,
                    Data = mapper.Map<FavouriteHeaderDto>(favouriteHeader),
                    SuccessMessage = SuccessMessage.ResourceManager.GetString("ProductSuccessfullyAddedToFavourite",
                        SuccessMessage.Culture),
                };
            }

            var favouriteDetails = detailsRepository
                .GetAll()
                .FirstOrDefault(key =>
                    key.ProductId == request.FavouriteDto.FavouriteDetails.First().ProductId &&
                    key.FavouriteHeaderId == favouriteHeaderFromDb.FavouriteHeaderId);

            if (favouriteDetails is null)
            {
                request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId =
                    favouriteHeaderFromDb.FavouriteHeaderId;

                await detailsRepository.CreateAsync(
                    mapper.Map<FavouriteDetails>(request.FavouriteDto.FavouriteDetails.First())!);
            }

            else
            {
                request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId =
                    favouriteDetails.FavouriteHeaderId;

                request.FavouriteDto.FavouriteDetails.First().FavouriteDetailsId =
                    favouriteDetails.FavouriteDetailsId;
            }

            var getAllHeaders1 = headerRepository.GetAll().ToList();
            var getAllDetails1 = detailsRepository.GetAll().ToList();

            memoryCache.Remove(getAllHeaders1);
            memoryCache.Remove(getAllDetails1);
            memoryCache.Set(CacheKey, getAllHeaders1);
            memoryCache.Set(CacheKey, getAllDetails1);

            return new Result<FavouriteHeaderDto>
            {
                StatusCode = (int)StatusCode.Created,
                Data = mapper.Map<FavouriteHeaderDto>(favouriteHeaderFromDb),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyAddedToFavourite",
                        SuccessMessage.Culture),
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