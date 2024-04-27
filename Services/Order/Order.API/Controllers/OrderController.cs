using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Requests.Commands;
using Order.Application.Features.Requests.Requests;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrderController : ControllerBase
{
    [HttpGet("GetOrders/{userId}")]
    public async Task<ActionResult<CollectionResult<OrderHeaderDto>>> GetOrders(ISender sender,
        ILogger<string> logger, [FromRoute] string userId)
    {
        var request = await sender.Send(new GetOrdersRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказы успешно получены: {userId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения заказов: {userId}");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    [HttpGet("GetOrder/{orderId:int}")]
    public async Task<ActionResult<Result<OrderHeaderDto>>> GetOrder(ISender sender, ILogger<int> logger,
        [FromRoute] int orderId)
    {
        var request = await sender.Send(new GetOrderRequest(orderId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказ успешно получен: {orderId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения заказа: {orderId}");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    [HttpPost("CreateOrder")]
    public async Task<ActionResult<Result<OrderHeaderDto>>> CreateOrder(ISender sender, ILogger<CartDto> logger,
        [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new CreateOrderRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Заказ успешно создан: {cartDto.CartHeader.CartHeaderId}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления заказа: {cartDto.CartHeader.CartHeaderId}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("CreateStripeSession")]
    public async Task<ActionResult<Result<StripeRequestDto>>> CreateStripeSession(ISender sender,
        ILogger<StripeRequestDto> logger, [FromBody] StripeRequestDto stripeRequestDto)
    {
        var command = await sender.Send(new CreateStripeSessionRequest(stripeRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Сессия для оплаты успешно создана: {stripeRequestDto.StripeSessionId}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка создания сессии для оплаты: {stripeRequestDto.StripeSessionId}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("ValidateStripeSession")]
    public async Task<ActionResult<Result<OrderHeaderDto>>> ValidateStripeSession(ISender sender,
        ILogger<ValidateStripeSessionDto> logger, [FromBody] ValidateStripeSessionDto validateStripeSessionDto)
    {
        var command = await sender.Send(new ValidateStripeSessionRequest(validateStripeSessionDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Оплата успешно прошла валидацию: {validateStripeSessionDto.OrderHeaderId}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Оплата не успешно прошла валидацию: {validateStripeSessionDto.OrderHeaderId}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("UpdateOrderStatus")]
    public async Task<ActionResult<Result<OrderHeaderDto>>> UpdateOrderStatus(ISender sender,
        ILogger<UpdateOrderStatusDto> logger, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
    {
        var command = await sender.Send(new UpdateOrderStatusRequest(updateOrderStatusDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Статус заказ успешно обновлен: {updateOrderStatusDto.OrderHeaderId} - {updateOrderStatusDto.NewStatus}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка обновления статуса заказа: {updateOrderStatusDto.OrderHeaderId} - {updateOrderStatusDto.NewStatus}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}