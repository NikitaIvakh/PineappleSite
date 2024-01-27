using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponDetailsRequest : IRequest<Result<CouponDto>>
    {
        public int Id { get; set; }
    }
}