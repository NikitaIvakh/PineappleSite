using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Resources;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponListRequestHandler(IBaseRepository<CouponEntity> couponRepository, ILogger logger) : IRequestHandler<GetCouponListRequest, CollectionResult<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _couponRepository = couponRepository;
        private readonly ILogger _logger = logger.ForContext<GetCouponListRequestHandler>();

        public async Task<CollectionResult<CouponDto>> Handle(GetCouponListRequest request, CancellationToken cancellationToken)
        {
            CouponDto[] coupons;

            try
            {
                coupons = await _couponRepository.GetAllAsync().Select(key => new CouponDto
                {
                    CouponId = key.CouponId,
                    CouponCode = key.CouponCode,
                    DiscountAmount = key.DiscountAmount,
                    MinAmount = key.MinAmount,
                }).ToArrayAsync(cancellationToken);
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

            if (coupons.Length == 0)
            {
                _logger.Warning(ErrorMessage.CouponsNotFound, coupons.Length);
                return new CollectionResult<CouponDto>()
                {
                    ErrorMessage = ErrorMessage.CouponsNotFound,
                    ErrorCode = (int)ErrorCodes.CouponsNotFound,
                };
            }

            return new CollectionResult<CouponDto>
            {
                Data = coupons,
                Count = coupons.Length
            };
        }
    }
}