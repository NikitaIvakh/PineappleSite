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

public sealed class GetCouponsRequestHandler(ICouponRepository couponRepository, IMemoryCache memoryCache)
    : IRequestHandler<GetCouponsRequest, CollectionResult<GetCouponsDto>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<CollectionResult<GetCouponsDto>> Handle(GetCouponsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<GetCouponsDto>? coupons))
            {
                if (coupons is not null)
                {
                    return new CollectionResult<GetCouponsDto>
                    {
                        Data = coupons,
                        Count = coupons!.Count,
                        StatusCode = (int)StatusCode.Ok,
                    };
                }
            }

            var couponsFromDb = await couponRepository.GetAllAsync().ToListAsync(cancellationToken);

            if (couponsFromDb.Count == 0)
            {
                memoryCache.Remove(CacheKey);
                return new CollectionResult<GetCouponsDto>()
                {
                    Data = [],
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("CouponsNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("CouponsNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ],
                };
            }

            var getCoupons = couponsFromDb.Select(key => new GetCouponsDto
            (
                CouponId: key.CouponId,
                CouponCode: key.CouponCode,
                DiscountAmount: key.DiscountAmount,
                MinAmount: key.MinAmount
            )).OrderByDescending(key => key.CouponId).ToList();

            memoryCache.Set(CacheKey, getCoupons);

            return new CollectionResult<GetCouponsDto>
            {
                Data = getCoupons,
                Count = getCoupons.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponsSuccessfullyGet", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new CollectionResult<GetCouponsDto>()
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}