using Coupon.Domain.DTOs;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponListRequest : IRequest<IEnumerable<CouponDto>>
    {

    }
}