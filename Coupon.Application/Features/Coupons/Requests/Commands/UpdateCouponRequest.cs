using Coupon.Application.DTOs;
using Coupon.Application.Response;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class UpdateCouponRequest : IRequest<BaseCommandResponse>
    {
        public UpdateCouponDto UpdateCoupon { get; set; }
    }
}