using AutoMapper;
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

public sealed class GetCouponsRequestHandler(
    ICouponRepository couponRepository,
    IMemoryCache memoryCache,
    IMapper mapper)
    : IRequestHandler<GetCouponsRequest, CollectionResult<CouponDto>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<CollectionResult<CouponDto>> Handle(GetCouponsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<CouponDto>? coupons))
            {
                if (coupons is not null)
                {
                    return new CollectionResult<CouponDto>
                    {
                        Data = coupons,
                        Count = coupons!.Count,
                        StatusCode = (int)StatusCode.Ok,
                    };
                }
            }

            var couponsFromDb = await couponRepository.GetAllAsync().OrderByDescending(key => key.CouponCode)
                .ToListAsync(cancellationToken);

            if (couponsFromDb.Count == 0)
            {
                memoryCache.Remove(CacheKey);
                return new CollectionResult<CouponDto>()
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

            memoryCache.Remove(CacheKey);

            return new CollectionResult<CouponDto>
            {
                Count = couponsFromDb.Count,
                StatusCode = (int)StatusCode.Ok,
                Data = mapper.Map<IReadOnlyCollection<CouponDto>>(couponsFromDb),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponsSuccessfullyGet", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new CollectionResult<CouponDto>()
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}