using Carter;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Coupon.API.Endpoints;

public class CouponEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/coupons");

        group.MapGet("", GetCoupons).WithName(nameof(GetCoupons));
        group.MapGet("{couponId:int}", GetCouponById).WithName(nameof(GetCouponById));
        group.MapGet("{couponCode}", GetCouponByCode).WithName(nameof(GetCouponByCode));
        group.MapPost("", CreateCoupon).WithName(nameof(CreateCoupon));
        group.MapPut("{couponId:int}", UpdateCoupon).WithName(nameof(UpdateCoupon));
        group.MapDelete("{couponId:int}", DeleteCoupon).WithName(nameof(DeleteCoupon));
        group.MapDelete("", DeleteCoupons).WithName(nameof(DeleteCoupons));
    }

    private static async Task<Results<Ok<CollectionResult<GetCouponsDto>>, BadRequest<string>>> GetCoupons(
        ISender sender)
    {
        var request = await sender.Send(new GetCouponsRequest());

        if (request.IsSuccess)
        {
            return TypedResults.Ok(request);
        }

        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<GetCouponDto>>, BadRequest<string>>> GetCouponById(ISender sender,
        int couponId)
    {
        var request = await sender.Send(new GetCouponRequest(couponId));

        if (request.IsSuccess)
        {
            return TypedResults.Ok(request);
        }

        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<GetCouponDto>>, BadRequest<string>>> GetCouponByCode(ISender sender,
        string couponCode)
    {
        var request = await sender.Send(new GetCouponByCodeRequest(couponCode));

        if (request.IsSuccess)
        {
            return TypedResults.Ok(request);
        }

        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<int>>, BadRequest<string>>> CreateCoupon(ISender sender,
        [FromBody] CreateCouponDto createCouponDto)
    {
        var command = await sender.Send(new CreateCouponRequest(createCouponDto));

        if (command.IsSuccess)
        {
            return TypedResults.Ok(command);
        }

        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> UpdateCoupon(ISender sender, int couponId,
        [FromBody] UpdateCouponDto updateCouponDto)
    {
        var command = await sender.Send(new UpdateCouponRequest(updateCouponDto));

        if (command.IsSuccess && couponId == updateCouponDto.CouponId)
        {
            return TypedResults.Ok(command);
        }

        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteCoupon(ISender sender, int couponId,
        [FromBody] DeleteCouponDto deleteCouponDto)
    {
        var command = await sender.Send(new DeleteCouponRequest(deleteCouponDto));
        {
            if (command.IsSuccess && couponId == deleteCouponDto.CouponId)
            {
                return TypedResults.Ok(command);
            }

            return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
        }
    }

    private static async Task<Results<Ok<CollectionResult<Unit>>, BadRequest<string>>> DeleteCoupons(ISender sender,
        [FromBody] DeleteCouponsDto deleteCouponsDto)
    {
        var command = await sender.Send(new DeleteCouponsRequest(deleteCouponsDto));

        if (command.IsSuccess)
        {
            return TypedResults.Ok(command);
        }

        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}