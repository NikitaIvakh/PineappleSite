using Coupon.Application.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponRequest : IRequest<Unit>
    {
        public DeleteCouponDto DeleteCoupon { get; set; }
    }
}