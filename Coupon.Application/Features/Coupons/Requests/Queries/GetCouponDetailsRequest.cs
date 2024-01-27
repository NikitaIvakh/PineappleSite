using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponDetailsRequest : IRequest<CouponDto>
    {
        public int Id { get; set; }
    }
}