using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries;

public sealed class GetCouponRequest(string couponId) : IRequest<Result<CouponDto>>
{
    public string CouponId { get; } = couponId;
}