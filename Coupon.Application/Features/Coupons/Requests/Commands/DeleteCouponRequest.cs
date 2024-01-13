using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponRequest : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}