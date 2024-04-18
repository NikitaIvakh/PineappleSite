using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands;

public class CreateCouponRequest(CreateCouponDto createCoupon) : IRequest<Result<int>>
{
    public CreateCouponDto CreateCoupon { get; } = createCoupon;
}