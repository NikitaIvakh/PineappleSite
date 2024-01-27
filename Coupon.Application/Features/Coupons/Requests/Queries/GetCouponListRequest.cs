using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries
{
    public class GetCouponListRequest : IRequest<CollectionResult<CouponDto>>
    {

    }
}