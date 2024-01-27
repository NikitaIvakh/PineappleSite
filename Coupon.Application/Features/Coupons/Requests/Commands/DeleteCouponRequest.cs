using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponRequest : IRequest<Result<CouponDto>>
    {
        public DeleteCouponDto DeleteCoupon { get; set; }
    }
}