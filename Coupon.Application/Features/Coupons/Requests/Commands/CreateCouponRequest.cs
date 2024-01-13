using Coupon.Application.DTOs;
using Coupon.Application.Response;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class CreateCouponRequest : IRequest<BaseCommandResponse>
    {
        public CreateCouponDto CreateCouponDto { get; set; }
    }
}