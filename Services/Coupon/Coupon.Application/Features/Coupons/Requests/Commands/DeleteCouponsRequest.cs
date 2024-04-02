using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands
{
    public class DeleteCouponsRequest(DeleteCouponsDto deleteCouponsDto) : IRequest<CollectionResult<bool>>
    {
        public DeleteCouponsDto DeleteCouponsDto { get; set; } = deleteCouponsDto;
    }
}