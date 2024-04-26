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
    IMemoryCache memoryCache) : IRequestHandler<RemoveCouponRequest, Result<CartHeaderDto>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<CartHeaderDto>> Handle(RemoveCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cartHeader = cartHeaderRepository.GetAll()
                .FirstOrDefault(key => key.UserId == request.CartDto.CartHeader.UserId);

            if (cartHeader is null)
            {
                return new Result<CartHeaderDto>
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

            return new Result<CartHeaderDto>
            {
                StatusCode = (int)StatusCode.Deleted,
                Data = mapper.Map<CartHeaderDto>(cartHeader),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<CartHeaderDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}