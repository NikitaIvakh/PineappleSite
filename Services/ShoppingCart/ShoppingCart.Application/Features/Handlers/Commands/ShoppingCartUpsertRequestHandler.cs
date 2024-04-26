using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class ShoppingCartUpsertRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<ShoppingCartUpsertRequest, Result<Unit>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<Unit>> Handle(ShoppingCartUpsertRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cartHeaderFromDb = cartHeaderRepository.GetAll()
                .FirstOrDefault(key =>
                    key.UserId == request.CartDto.CartHeader.UserId &&
                    key.CartHeaderId == request.CartDto.CartHeader.CartHeaderId);

            if (cartHeaderFromDb is null)
            {
                var cartHeader = mapper.Map<CartHeader>(request.CartDto.CartHeader);
                await cartHeaderRepository.CreateAsync(cartHeader);

                request.CartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                await cartDetailsRepository.CreateAsync(
                    mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));

                memoryCache.Remove(CacheKey);

                return new Result<Unit>
                {
                    Data = Unit.Value,
                    StatusCode = (int)StatusCode.Created,
                    SuccessMessage = SuccessMessage.ResourceManager.GetString("ProductSuccessfullyAddedShoppingCart",
                        SuccessMessage.Culture),
                };
            }

            var cartDetailsFromDb = cartDetailsRepository
                .GetAll()
                .FirstOrDefault(key =>
                    key.ProductId == request.CartDto.CartDetails.First().ProductId &&
                    key.CartHeaderId == cartHeaderFromDb.CartHeaderId &&
                    key.CartHeader!.UserId == cartHeaderFromDb.UserId);

            if (cartDetailsFromDb is null)
            {
                request.CartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                await cartDetailsRepository.CreateAsync(
                    mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
            }

            else
            {
                request.CartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                request.CartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                request.CartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                await cartDetailsRepository.UpdateAsync(
                    mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyAddedShoppingCart",
                        SuccessMessage.Culture),
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