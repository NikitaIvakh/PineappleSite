﻿using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Resources;
using Coupon.Domain.DTOs;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponByCodeRequestHandler(ICouponRepository repository, IMemoryCache memoryCache) 
        : IRequestHandler<GetCouponByCodeRequest, Result<GetCouponDto>>
    {
        private const string CacheKey = "couponsCacheKey";

        public async Task<Result<GetCouponDto>> Handle(GetCouponByCodeRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (memoryCache.TryGetValue(CacheKey, out GetCouponDto? coupon))
                {
                    if (coupon is null)
                    {
                        return new Result<GetCouponDto>
                        {
                            Data = coupon,
                            StatusCode = (int)StatusCode.NoContent,
                        };
                    }
                }

                var couponFromDb = await repository.GetAllAsync()
                    .FirstOrDefaultAsync(key => key.CouponCode == request.CouponCode, cancellationToken);

                if (couponFromDb is null)
                {
                    memoryCache.Remove(CacheKey);
                    return new Result<GetCouponDto>
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage = ErrorMessage.CouponNotFound,
                        ValidationErrors = [ErrorMessage.CouponNotFound]
                    };
                }

                memoryCache.Set(CacheKey, couponFromDb);

                return new Result<GetCouponDto>
                {
                    Data = new GetCouponDto
                    (
                        CouponId: couponFromDb.CouponId,
                        CouponCode: couponFromDb.CouponCode,
                        DiscountAmount: couponFromDb.DiscountAmount,
                        MinAmount: couponFromDb.MinAmount
                    ),
                    
                    StatusCode = (int)StatusCode.NoContent,
                };
            }

            catch (Exception exception)
            {
                memoryCache.Remove(CacheKey);
                return new Result<GetCouponDto>
                {
                    ErrorMessage = exception.Message,
                    StatusCode = (int)StatusCode.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}