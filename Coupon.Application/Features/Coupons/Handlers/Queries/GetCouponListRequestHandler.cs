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

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponListRequestHandler(IBaseRepository<CouponEntity> couponRepository, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetCouponListRequest, CollectionResult<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _couponRepository = couponRepository;
        private readonly ILogger _logger = logger.ForContext<GetCouponListRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponsCacheKey";

        public async Task<CollectionResult<CouponDto>> Handle(GetCouponListRequest request, CancellationToken cancellationToken)
        {
            try
            {

                if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<CouponDto> coupons))
                {
                    return new CollectionResult<CouponDto>
                    {
                        Data = coupons,
                        Count = coupons.Count
                    };
                }

                else
                {
                    coupons = await _couponRepository.GetAllAsync().Select(key => new CouponDto
                    {
                        CouponId = key.CouponId,
                        CouponCode = key.CouponCode,
                        DiscountAmount = key.DiscountAmount,
                        MinAmount = key.MinAmount,
                    }).OrderBy(key => key.CouponId).ToListAsync(cancellationToken);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                    _memoryCache.Set(cacheKey, coupons, cacheEntryOptions);

                    if (coupons is null || coupons.Count == 0)
                    {
                        _logger.Warning(ErrorMessage.CouponsNotFound, coupons.Count);
                        _memoryCache.Remove(cacheKey);
                        return new CollectionResult<CouponDto>()
                        {
                            Data = [],
                            SuccessMessage = "Никаких купонов нет"
                        };
                    }

                    else
                    {
                        return new CollectionResult<CouponDto>
                        {
                            Data = coupons,
                            Count = coupons.Count
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new CollectionResult<CouponDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}