using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.API.Endpoints;

public class ShoppingCartEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/shoppingCart");

        group.MapGet("/GetShoppingCart/{userId}", GetShoppingCart);
        group.MapPost("/ShoppingCartUpsert", ShoppingCartUpsert);
        group.MapPost("/ApplyCoupon", ApplyCoupon);
        group.MapDelete("/RemoveCoupon", RemoveCoupon);
        group.MapDelete("/RemoveProduct", RemoveProduct);
        group.MapPost("/SendMessage", SendMessage);
    }

    private static async Task<Results<Ok<Result<CartDto>>, BadRequest<string>>> GetShoppingCart(ISender sender,
        ILogger<string> logger, [FromRoute] string userId)
    {
        var request = await sender.Send(new GetShoppingCartRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Корзина успешно получена: {userId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения корзины: {userId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<CartHeaderDto>>, BadRequest<string>>> ShoppingCartUpsert(ISender sender,
        ILogger<CartDto> logger, [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new ShoppingCartUpsertRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ В корзину успешно добавлен товар: {cartDto.CartHeader.CartHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка добавления товара в корзину: {cartDto.CartHeader.CartHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<CartHeaderDto>>, BadRequest<string>>> ApplyCoupon(ISender sender,
        ILogger<CartDto> logger, [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new ApplyCouponRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Купон успешно применен: {cartDto.CartHeader.CartHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка применения купона: {cartDto.CartHeader.CartHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<CartHeaderDto>>, BadRequest<string>>> RemoveCoupon(ISender sender,
        ILogger<CartDto> logger, [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new RemoveCouponRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно удален: {cartDto.CartHeader.CartHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления купона: {cartDto.CartHeader.CartHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> RemoveProduct(ISender sender,
        ILogger<int> logger, [FromRoute] int productId)
    {
        var command = await sender.Send(new RemoveShoppingCartProductRequest(productId));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукт из корзины успешно удален: {productId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления продукта из корзины: {productId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<CollectionResult<bool>>, BadRequest<string>>> RemoveProducts(ISender sender,
        ILogger<DeleteProductsDto> logger, [FromBody] DeleteProductsDto deleteProductsDto)
    {
        var command = await sender.Send(new RemoveShoppingCartProductsRequest(deleteProductsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Продукты из корзины успешно удалены: {deleteProductsDto.ProductIds}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления продуктов из корзины: {deleteProductsDto.ProductIds}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<bool>>, BadRequest<string>>> SendMessage(ISender sender,
        ILogger<CartDto> logger, [FromBody] CartDto cartDto)
    {
        var command = await sender.Send(new RabbitMqSendRequest(cartDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Письмо в очередь успешно доставлено: {cartDto.CartHeader.CartHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка добавления письма в очередь: {cartDto.CartHeader.CartHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}