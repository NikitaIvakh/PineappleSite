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
    public class GetCouponDetailsByCouponNameRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetCouponDetailsByCouponNameRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetCouponDetailsByCouponNameRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "couponCodeCacheKey";

        public async Task<Result<CouponDto>> Handle(GetCouponDetailsByCouponNameRequest request, CancellationToken cancellationToken)
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

                coupon = await _repository.GetAllAsync().Select(key => new CouponDto
                {
                    CouponId = key.CouponId,
                    CouponCode = key.CouponCode,
                    DiscountAmount = key.DiscountAmount,
                    MinAmount = key.MinAmount,
                }).FirstOrDefaultAsync(key => key.CouponCode.ToLower() == request.CouponCode.ToLower(), cancellationToken);

                if (coupon is null)
                {
                    _logger.Warning($"Купон с {request.CouponCode} не найден");
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