using Coupon.Application.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class UpdateCouponRequest : IRequest<Unit>
    {
        public int Id { get; set; }

        public UpdateCouponDto UpdateCoupon { get; set; }
    }
}