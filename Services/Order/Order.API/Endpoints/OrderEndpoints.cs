using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Requests.Commands;
using Order.Application.Features.Requests.Requests;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;
using static Order.API.Utility.StaticDetails;

namespace Order.API.Endpoints;

public sealed class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/order").RequireAuthorization(UserAndAdministratorPolicy);

        group.MapGet("/GetOrders/{userId}", GetOrders);
        group.MapGet("/GetOrder/{orderId:int}", GetOrder);
        group.MapPost("/CreateOrder", CreateOrder);
        group.MapPost("/CreateStripeSession", CreateStripeSession);
        group.MapPost("/ValidateStripeSession", ValidateStripeSession);
        group.MapPost("/UpdateOrderStatus", UpdateOrderStatus);
    }

    private static async Task<Results<Ok<CollectionResult<OrderHeaderDto>>, BadRequest<string>>> GetOrders(
        ISender sender, ILogger<string> logger, [FromRoute] string userId)
    {
        var request = await sender.Send(new GetOrdersRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказы успешно получены: {userId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения заказов: {userId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<OrderHeaderDto>>, BadRequest<string>>> GetOrder(ISender sender,
        ILogger<int> logger, [FromRoute] int orderId)
    {
        var request = await sender.Send(new GetOrderRequest(orderId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказ успешно получен: {orderId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения заказа: {orderId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<OrderHeaderDto>>, BadRequest<string>>> CreateOrder(ISender sender,
        ILogger<CartDto> logger, [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new CreateOrderRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказ успешно создан: {cartDto.CartHeader.CartHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления заказа: {cartDto.CartHeader.CartHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<StripeRequestDto>>, BadRequest<string>>> CreateStripeSession(
        ISender sender,
        ILogger<StripeRequestDto> logger, [FromBody] StripeRequestDto stripeRequestDto)
    {
        var command = await sender.Send(new CreateStripeSessionRequest(stripeRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Сессия для оплаты успешно создана: {stripeRequestDto.StripeSessionId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка создания сессии для оплаты: {stripeRequestDto.StripeSessionId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<OrderHeaderDto>>, BadRequest<string>>> ValidateStripeSession(
        ISender sender, ILogger<ValidateStripeSessionDto> logger,
        [FromBody] ValidateStripeSessionDto validateStripeSessionDto)
    {
        var command = await sender.Send(new ValidateStripeSessionRequest(validateStripeSessionDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Оплата успешно прошла валидацию: {validateStripeSessionDto.OrderHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Оплата не успешно прошла валидацию: {validateStripeSessionDto.OrderHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<OrderHeaderDto>>, BadRequest<string>>> UpdateOrderStatus(ISender sender,
        ILogger<UpdateOrderStatusDto> logger, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
    {
        var command = await sender.Send(new UpdateOrderStatusRequest(updateOrderStatusDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Статус заказ успешно обновлен: {updateOrderStatusDto.OrderHeaderId} - {updateOrderStatusDto.NewStatus}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка обновления статуса заказа: {updateOrderStatusDto.OrderHeaderId} - {updateOrderStatusDto.NewStatus}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}