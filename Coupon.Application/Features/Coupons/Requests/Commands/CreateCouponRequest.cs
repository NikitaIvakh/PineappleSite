using Coupon.Application.Response;
using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class CreateCouponRequest : IRequest<BaseCommandResponse>
    {
        public CreateCouponDto CreateCouponDto { get; set; }
    }
}