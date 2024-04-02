using Carter;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Coupon.API.Utility.StaticDetails;

namespace Coupon.API.Endpoints;

public class CouponEndpoints(ILogger<GetCouponsDto> coupons, ILogger<GetCouponDto> coupon) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/coupons");

        group.MapGet("", GetCoupons).WithName(nameof(GetCoupons)).RequireAuthorization(RoleAdministrator);
        group.MapGet("{couponId:int}", GetCouponById).WithName(nameof(GetCouponById));
        group.MapGet("{couponCode}", GetCouponByCode).WithName(nameof(GetCouponByCode));
        group.MapPost("", CreateCoupon).WithName(nameof(CreateCoupon)).RequireAuthorization(RoleAdministrator);
        group.MapPut("{couponId:int}", UpdateCoupon).WithName(nameof(UpdateCoupon)).RequireAuthorization(RoleAdministrator);
        group.MapDelete("{couponId:int}", DeleteCoupon).WithName(nameof(DeleteCoupon)).RequireAuthorization(RoleAdministrator);
        group.MapDelete("", DeleteCoupons).WithName(nameof(DeleteCoupons)).RequireAuthorization(RoleAdministrator);
    }

    private async Task<Results<Ok<CollectionResult<GetCouponsDto>>, BadRequest<string>>> GetCoupons(
        ISender sender)
    {
        var request = await sender.Send(new GetCouponsRequest());

        if (request.IsSuccess)
        {
            coupons.LogDebug("LogDebug ====================== Купоны успешно получены");
            return TypedResults.Ok(request);
        }
        
        coupons.LogDebug("LogDebug ====================== Ошибка получения купонов");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private async Task<Results<Ok<Result<GetCouponDto>>, BadRequest<string>>> GetCouponById(ISender sender,
        int couponId)
    {
        var request = await sender.Send(new GetCouponRequest(couponId));

        if (request.IsSuccess)
        {
            coupon.LogDebug($"LogDebug ====================== Купон успешно получен:{couponId}");
            return TypedResults.Ok(request);
        }

        coupon.LogDebug($"LogDebug ====================== Ошибка получения купона:{couponId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private async Task<Results<Ok<Result<GetCouponDto>>, BadRequest<string>>> GetCouponByCode(ISender sender,
        string couponCode)
    {
        var request = await sender.Send(new GetCouponByCodeRequest(couponCode));

        if (request.IsSuccess)
        {
            coupon.LogDebug($"LogDebug ====================== Купон успешно получен:{couponCode}");
            return TypedResults.Ok(request);
        }

        coupon.LogDebug($"LogDebug ====================== Ошибка получения купона:{couponCode}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private async Task<Results<Ok<Result<int>>, BadRequest<string>>> CreateCoupon(ISender sender,
        [FromBody] CreateCouponDto createCouponDto)
    {
        var command = await sender.Send(new CreateCouponRequest(createCouponDto));

        if (command.IsSuccess)
        {
            coupon.LogDebug($"LogDebug ====================== Купон успешно создан");
            return TypedResults.Ok(command);
        }

        coupon.LogDebug($"LogDebug ====================== Ошибка создания купона");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> UpdateCoupon(ISender sender, int couponId,
        [FromBody] UpdateCouponDto updateCouponDto)
    {
        var command = await sender.Send(new UpdateCouponRequest(updateCouponDto));

        if (command.IsSuccess && couponId == updateCouponDto.CouponId)
        {
            coupon.LogDebug($"LogDebug ====================== Купон успешно обновлен");
            return TypedResults.Ok(command);
        }

        coupon.LogDebug($"LogDebug ====================== Ошибка обновления купона");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteCoupon(ISender sender, int couponId,
        [FromBody] DeleteCouponDto deleteCouponDto)
    {
        var command = await sender.Send(new DeleteCouponRequest(deleteCouponDto));
        {
            if (command.IsSuccess && couponId == deleteCouponDto.CouponId)
            {
                coupon.LogDebug($"LogDebug ====================== Купон успешно удален:{couponId}");
                return TypedResults.Ok(command);
            }

            coupon.LogDebug($"LogDebug ====================== Ошибка удаления купона:{couponId}");
            return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
        }
    }

    private async Task<Results<Ok<CollectionResult<Unit>>, BadRequest<string>>> DeleteCoupons(ISender sender,
        [FromBody] DeleteCouponsDto deleteCouponsDto)
    {
        var command = await sender.Send(new DeleteCouponsRequest(deleteCouponsDto));

        if (command.IsSuccess)
        {
            coupon.LogDebug($"LogDebug ====================== Купоны успешно удалены");
            return TypedResults.Ok(command);
        }

        coupon.LogDebug($"LogDebug ====================== Ошибка удаления купонов");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}