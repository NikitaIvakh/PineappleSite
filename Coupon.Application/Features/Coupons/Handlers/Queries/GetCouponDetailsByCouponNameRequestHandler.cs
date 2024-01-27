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
    public class GetCouponDetailsByCouponNameRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger) : IRequestHandler<GetCouponDetailsByCouponNameRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetCouponDetailsByCouponNameRequestHandler>();

        public async Task<Result<CouponDto>> Handle(GetCouponDetailsByCouponNameRequest request, CancellationToken cancellationToken)
        {
            CouponDto coupon;

            try
            {
                coupon = await _repository.GetAllAsync().Select(key => new CouponDto
                {
                    CouponId = key.CouponId,
                    CouponCode = key.CouponCode,
                    DiscountAmount = key.DiscountAmount,
                    MinAmount = key.MinAmount,
                }).FirstAsync(key => key.CouponCode.ToLower() == request.CouponCode.ToLower(), cancellationToken);
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
                _logger.Warning("Купон с {CouponCode} не найден", request.CouponCode);
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