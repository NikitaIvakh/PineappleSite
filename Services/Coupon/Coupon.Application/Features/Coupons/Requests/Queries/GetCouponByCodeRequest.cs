using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Queries;

public sealed class GetCouponByCodeRequest(string couponCode) : IRequest<Result<CouponDto>>
{
    public string CouponCode { get; } = couponCode;
}