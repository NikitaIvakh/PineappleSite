﻿using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;

namespace Coupon.Application.Features.Coupons.Requests.Commands;

public sealed class DeleteCouponRequest(DeleteCouponDto deleteCouponDto) : IRequest<Result<Unit>>
{
    public DeleteCouponDto DeleteCouponDto { get; } = deleteCouponDto;
}