using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries;

public class GetCouponRequest(int couponId) : IRequest<Result<GetCouponDto>>
{
    public int CouponId { get; } = couponId;
}