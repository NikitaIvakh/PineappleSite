using Coupon.Application.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class CreateCouponRequest : IRequest<int>
    {
        public CreateCouponDto CreateCouponDto { get; set; }
    }
}