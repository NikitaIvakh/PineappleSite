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
    public class GetCouponDetailsRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger) : IRequestHandler<GetCouponDetailsRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetCouponDetailsRequestHandler>();

        public async Task<Result<CouponDto>> Handle(GetCouponDetailsRequest request, CancellationToken cancellationToken)
        {
            CouponDto? coupon;

            try
            {
                coupon = await _repository.GetAllAsync().Select(key => new CouponDto
                {
                    CouponId = key.CouponId,
                    CouponCode = key.CouponCode,
                    DiscountAmount = key.DiscountAmount,
                    MinAmount = key.MinAmount,
                }).FirstOrDefaultAsync(key => key.CouponId == request.Id, cancellationToken);
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

            if (coupon is null)
            {
                _logger.Warning($"Купон с {request.Id} не найден");
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.CouponNotFound,
                    ErrorCode = (int)ErrorCodes.CouponNotFound1,
                };
            }

            return new Result<CouponDto>
            {
                Data = coupon,
            };
        }
    }
}