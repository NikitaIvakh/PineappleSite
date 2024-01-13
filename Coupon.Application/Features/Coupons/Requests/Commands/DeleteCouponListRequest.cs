using Coupon.Application.DTOs;
using Coupon.Application.Response;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponListRequest : IRequest<BaseCommandResponse>
    {
        public DeleteCouponListDto DeleteCoupon { get; set; }
    }
}