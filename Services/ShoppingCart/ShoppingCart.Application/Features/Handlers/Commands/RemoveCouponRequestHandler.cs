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

public sealed class RemoveCouponRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IMapper mapper,
    IMemoryCache memoryCache) : IRequestHandler<RemoveCouponRequest, Result<Unit>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<Unit>> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cartHeader = cartHeaderRepository.GetAll()
                .FirstOrDefault(key =>
                    key.UserId == request.CartDto.CartHeader.UserId &&
                    key.CartHeaderId == request.CartDto.CartHeader.CartHeaderId);

            if (cartHeader is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("CartHeaderNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("CartHeaderNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            cartHeader.CouponCode = null;
            await cartHeaderRepository.UpdateAsync(cartHeader);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyDeleted", SuccessMessage.Culture),
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