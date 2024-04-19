using AutoMapper;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Application.Resources;
using Favourite.Domain.DTOs;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Queries;

public sealed class GetFavouriteProductsRequestHandler(
    IBaseRepository<FavouriteHeader> headerRepository,
    IBaseRepository<FavouriteDetails> detailsRepository,
    IProductService productService,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<GetFavouriteProductsRequest, Result<FavouriteDto>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<FavouriteDto>> Handle(GetFavouriteProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out FavouriteDto? favouriteDto))
            {
                return new Result<FavouriteDto>
                {
                    Data = favouriteDto,
                    SuccessMessage = "Ваши избранные товары",
                };
            }

            var favouriteHeader = headerRepository.GetAll().Select(key => new FavouriteHeaderDto
            {
                FavouriteHeaderId = key.FavouriteHeaderId,
                UserId = key.UserId,
            }).FirstOrDefault(key => key.UserId == request.UserId);

            if (favouriteHeader is null)
            {
                return new Result<FavouriteDto>
                {
                    Data = new FavouriteDto
                    (
                        favouriteHeader: new FavouriteHeaderDto(),
                        favouriteDetails: []
                    ),

                    StatusCode = (int)StatusCode.NoAction,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ThereAreNoFavouriteProducts", SuccessMessage.Culture),
                };
            }

            var favouriteDetails = detailsRepository.GetAll().Select(key =>
                new FavouriteDetailsDto
                {
                    FavouriteDetailsId = key.FavouriteDetailsId,
                    FavouriteHeader = mapper.Map<FavouriteHeaderDto>(key.FavouriteHeader),
                    FavouriteHeaderId = key.FavouriteHeaderId,
                    Product = key.Product,
                    ProductId = key.ProductId,
                }).OrderByDescending(key => key.FavouriteDetailsId).ToList();

            if (favouriteDetails.Count == 0)
            {
                return new Result<FavouriteDto>
                {
                    Data = new FavouriteDto
                    (
                        favouriteHeader: new FavouriteHeaderDto(),
                        favouriteDetails: []
                    ),

                    StatusCode = (int)StatusCode.NoAction,
                    SuccessMessage = SuccessMessage.ThereAreNoFavouriteProducts,
                };
            }

            var getFavouriteDto = new FavouriteDto(favouriteHeader, favouriteDetails);
            var products = await productService.GetProductListAsync();

            foreach (var item in getFavouriteDto.FavouriteDetails)
            {
                item.Product = products?.Data?.FirstOrDefault(key => key.Id == item.ProductId);
            }

            var getAllHeaders = headerRepository.GetAll().ToList();
            var getAllDetails = detailsRepository.GetAll().ToList();

            memoryCache.Remove(getAllHeaders);
            memoryCache.Remove(getAllDetails);
            memoryCache.Set(CacheKey, getAllHeaders);
            memoryCache.Set(CacheKey, getAllDetails);

            return new Result<FavouriteDto>
            {
                Data = getFavouriteDto,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage = SuccessMessage.FavouriteProducts,
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<FavouriteDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}