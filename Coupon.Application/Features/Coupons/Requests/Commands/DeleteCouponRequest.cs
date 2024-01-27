using Coupon.Application.Response;
using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponRequest : IRequest<BaseCommandResponse>
    {
        public DeleteCouponDto DeleteCoupon { get; set; }
    }
}