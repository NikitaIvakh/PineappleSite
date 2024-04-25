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

public sealed class GetCouponRequestHandler(ICouponRepository repository, IMemoryCache memoryCache, IMapper mapper)
    : IRequestHandler<GetCouponRequest, Result<CouponDto>>
{
    private const string CacheKey = "couponsCacheKey";

    public async Task<Result<CouponDto>> Handle(GetCouponRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out CouponDto? coupon))
            {
                if (coupon is not null)
                {
                    return new Result<CouponDto>
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
                return new Result<CouponDto>
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

            memoryCache.Remove(CacheKey);

            return new Result<CouponDto>
            {
                StatusCode = (int)StatusCode.Ok,
                Data = mapper.Map<CouponDto>(couponFromDb),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("CouponSuccessfullyGet", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<CouponDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}