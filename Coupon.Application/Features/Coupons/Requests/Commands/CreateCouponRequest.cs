using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class CreateCouponRequest : IRequest<Result<CouponDto>>
    {
        public CreateCouponDto CreateCoupon { get; set; }
    }
}