﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class ApplyCouponRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IMapper mapper,
    ICouponService couponService,
    IMemoryCache memoryCache) : IRequestHandler<ApplyCouponRequest, Result<CartHeaderDto>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<CartHeaderDto>> Handle(ApplyCouponRequest request, CancellationToken cancellationToken)
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

            var coupon = await couponService.GetCouponAsync(request.CartDto.CartHeader.CouponCode);

            if (coupon.Data is null)
            {
                return new Result<CartHeaderDto>
                {
                    Data = mapper.Map<CartHeaderDto>(cartHeader),
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("CouponNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("CouponNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            cartHeader.CouponCode = request.CartDto.CartHeader.CouponCode?.Trim();
            await cartHeaderRepository.UpdateAsync(cartHeader);
            memoryCache.Remove(CacheKey);


            return new Result<CartHeaderDto>
            {
                StatusCode = (int)StatusCode.Modify,
                Data = mapper.Map<CartHeaderDto>(cartHeader),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyApplied", SuccessMessage.Culture),
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