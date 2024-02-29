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
using Serilog;
using Stripe;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponDetailsRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetCouponDetailsRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetCouponDetailsRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponCacheKey";

        public async Task<Result<CouponDto>> Handle(GetCouponDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out CouponDto? coupon))
                {
                    return new Result<CouponDto>
                    {
                        Data = coupon,
                    };
                }

                else
                {
                    coupon = await _repository.GetAllAsync().Select(key => new CouponDto
                    {
                        CouponId = key.CouponId,
                        CouponCode = key.CouponCode,
                        DiscountAmount = key.DiscountAmount,
                        MinAmount = key.MinAmount,
                    }).FirstOrDefaultAsync(key => key.CouponId == request.Id, cancellationToken);

                    _memoryCache.Set(cacheKey, coupon);

                    if (coupon is null)
                    {
                        _logger.Warning($"Купон с {request.Id} не найден");
                        _memoryCache.Remove(cacheKey);
                        return new Result<CouponDto>
                        {
                            ErrorMessage = ErrorMessage.CouponNotFound,
                            ErrorCode = (int)ErrorCodes.CouponNotFound,
                        };
                    }

                    else
                    {
                        return new Result<CouponDto>
                        {
                            Data = coupon,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}