using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Resources;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponRequestHandler(IBaseRepository<CouponEntity> repository, IMemoryCache memoryCache) 
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
                            SuccessCode = (int)SuccessCode.Ok,
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
                        ErrorMessage = ErrorMessage.CouponNotFound,
                        ErrorCode = (int)ErrorCodes.CouponNotFound,
                        ValidationErrors = [ErrorMessage.CouponNotFound]
                    };
                }

                memoryCache.Set(CacheKey, coupon);

                return new Result<GetCouponDto>
                {
                    Data = new GetCouponDto
                        (
                            CouponId: couponFromDb!.CouponId, 
                            CouponCode: couponFromDb.CouponCode, 
                            DiscountAmount: couponFromDb.DiscountAmount, 
                            MinAmount: couponFromDb.MinAmount
                        ),
                    
                    SuccessCode = (int)SuccessCode.Ok,
                };
            }

            catch (Exception exception)
            {
                memoryCache.Remove(CacheKey);
                return new Result<GetCouponDto>
                {
                    ErrorMessage = exception.Message,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}