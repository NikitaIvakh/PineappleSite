﻿using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands;

public sealed class UpdateCouponRequest(UpdateCouponDto updateCouponDto) : IRequest<Result<Unit>>
{
    public UpdateCouponDto UpdateCouponDto { get; } = updateCouponDto;
}