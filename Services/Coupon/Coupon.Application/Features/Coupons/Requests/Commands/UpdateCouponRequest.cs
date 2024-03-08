using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class UpdateCouponRequest : IRequest<Result<CouponDto>>
    {
        public UpdateCouponDto UpdateCoupon { get; set; }
    }
}