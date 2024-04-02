using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class UpdateCouponRequest(UpdateCouponDto updateCouponDto) : IRequest<Result<Unit>>
    {
        public UpdateCouponDto UpdateCouponDto { get; set; } = updateCouponDto;
    }
}