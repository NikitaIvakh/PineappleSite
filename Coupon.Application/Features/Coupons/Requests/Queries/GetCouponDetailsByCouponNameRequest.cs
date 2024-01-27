using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponDetailsByCouponNameRequest : IRequest<Result<CouponDto>>
    {
        public string CouponCode { get; set; }
    }
}