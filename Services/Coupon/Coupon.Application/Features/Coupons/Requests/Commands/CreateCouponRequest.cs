using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands;

public sealed class CreateCouponRequest(CreateCouponDto createCoupon) : IRequest<Result<string>>
{
    public CreateCouponDto CreateCoupon { get; } = createCoupon;
}