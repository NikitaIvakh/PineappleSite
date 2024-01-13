using Coupon.Application.DTOs;
using Coupon.Application.Response;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponRequest : IRequest<BaseCommandResponse>
    {
        public DeleteCouponDto DeleteCoupon { get; set; }
    }
}