using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Resources;
using Coupon.Domain.DTOs;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Application.Features.Coupons.Handlers.Queries;

public class GetCouponRequestHandler(ICouponRepository repository, IMemoryCache memoryCache)
    : IRequestHandler<GetCouponRequest, Result<GetCouponDto>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<Result<GetCouponDto>> Handle(GetCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out GetCouponDto? coupon))
            {
                if (coupon is not null)
                {
                    return new Result<GetCouponDto>
                    {
                        Data = coupon,
                        StatusCode = (int)StatusCode.Ok,
                    };
                }
            }

            var couponFromDb = await repository.GetAllAsync()
                .FirstOrDefaultAsync(key => key.CouponId == request.CouponId, cancellationToken);

            if (couponFromDb is null)
            {
                memoryCache.Remove(CacheKey);
                return new Result<GetCouponDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("CouponNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var getCoupon = new GetCouponDto
            (
                CouponId: couponFromDb!.CouponId,
                CouponCode: couponFromDb.CouponCode,
                DiscountAmount: couponFromDb.DiscountAmount,
                MinAmount: couponFromDb.MinAmount
            );

            memoryCache.Set(CacheKey, coupon);

            return new Result<GetCouponDto>
            {
                Data = getCoupon,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyGet", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<GetCouponDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}