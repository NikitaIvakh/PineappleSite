using Coupon.Application.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponDetailsByCouponNameRequest : IRequest<CouponDto>
    {
        public string CouponCode { get; set; }
    }
}