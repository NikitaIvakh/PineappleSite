using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries;

public sealed class GetCouponsRequest : IRequest<CollectionResult<CouponDto>>
{

}