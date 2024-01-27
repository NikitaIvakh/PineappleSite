using Coupon.Application.Response;
using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponListRequest : IRequest<BaseCommandResponse>
    {
        public DeleteCouponListDto DeleteCoupon { get; set; }
    }
}