using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries;

public class GetCouponRequest(string couponId) : IRequest<Result<GetCouponDto>>
{
    public string CouponId { get; } = couponId;
}