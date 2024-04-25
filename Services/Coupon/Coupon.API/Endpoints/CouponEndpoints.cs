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

public sealed class CouponEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/coupons");

        group.MapGet("/GetCoupons", GetCoupons).RequireAuthorization(AdministratorPolicy);
        group.MapGet("/GetCouponById/{couponId}", GetCouponById).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapGet("/GetCouponByCode/{couponCode}", GetCouponByCode).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapPost("/CreateCoupon", CreateCoupon).RequireAuthorization(AdministratorPolicy);
        group.MapPut("/UpdateCoupon/{couponId}", UpdateCoupon).RequireAuthorization(AdministratorPolicy);
        group.MapDelete("/DeleteCoupon/{couponId}", DeleteCoupon).RequireAuthorization(AdministratorPolicy);
        group.MapDelete("/DeleteCoupons", DeleteCoupons).RequireAuthorization(AdministratorPolicy);
    }

    private static async Task<Results<Ok<CollectionResult<CouponDto>>, BadRequest<string>>> GetCoupons(
        ISender sender, ILogger<CouponDto> logger)
    {
        var request = await sender.Send(new GetCouponsRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug("LogDebug ================ Купоны успешно получены");
            return TypedResults.Ok(request);
        }

        logger.LogError("LogDebugError ================ Ошибка получения купонов");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<CouponDto>>, BadRequest<string>>> GetCouponById(string couponId,
        ISender sender, ILogger<CouponDto> logger)
    {
        var request = await sender.Send(new GetCouponRequest(couponId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно получен: {couponId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения купона: {couponId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<CouponDto>>, BadRequest<string>>> GetCouponByCode(string couponCode,
        ISender sender, ILogger<CouponDto> logger)
    {
        var request = await sender.Send(new GetCouponByCodeRequest(couponCode));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно получен: {couponCode}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения купона: {couponCode}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<string>>, BadRequest<string>>> CreateCoupon(ISender sender,
        ILogger<CreateCouponDto> logger, [FromBody] CreateCouponDto createCouponDto)
    {
        var command = await sender.Send(new CreateCouponRequest(createCouponDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно создан: {createCouponDto.CouponCode}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка создания купона: {createCouponDto.CouponCode}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> UpdateCoupon(string couponId,
        ISender sender,
        ILogger<UpdateCouponDto> logger, [FromBody] UpdateCouponDto updateCouponDto)
    {
        var command = await sender.Send(new UpdateCouponRequest(updateCouponDto));

        if (command.IsSuccess && couponId == updateCouponDto.CouponId)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно обновлен: {updateCouponDto.CouponId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка обновления купона: {updateCouponDto.CouponId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteCoupon(string couponId,
        ISender sender,
        ILogger<DeleteCouponDto> logger, [FromBody] DeleteCouponDto deleteCouponDto)
    {
        var command = await sender.Send(new DeleteCouponRequest(deleteCouponDto));

        if (command.IsSuccess && couponId == deleteCouponDto.CouponId)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно удален: {deleteCouponDto.CouponId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления купона: {deleteCouponDto.CouponId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<CollectionResult<bool>>, BadRequest<string>>> DeleteCoupons(ISender sender,
        ILogger<DeleteCouponsDto> logger, [FromBody] DeleteCouponsDto deleteCouponsDto)
    {
        var command = await sender.Send(new DeleteCouponsRequest(deleteCouponsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купоны успешно удалены: {deleteCouponsDto.CouponIds}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления купонов: {deleteCouponsDto.CouponIds}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}