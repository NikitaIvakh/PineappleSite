using Coupon.Application.Response;
using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class UpdateCouponRequest : IRequest<BaseCommandResponse>
    {
        public UpdateCouponDto UpdateCoupon { get; set; }
    }
}