using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;
using static ShoppingCart.API.Utility.StaticDetails;

namespace ShoppingCart.API.Endpoints;

public class ShoppingCartEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/shoppingCart");

        group.MapGet("/GetShoppingCart/{userId}", GetShoppingCart).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapPost("/ShoppingCartUpsert", ShoppingCartUpsert).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapPost("/ApplyCoupon", ApplyCoupon).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapPost("/RemoveCoupon", RemoveCoupon).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapDelete("/RemoveProduct", RemoveProduct).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapDelete("/RemoveProductByUser", RemoveProductByUser).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapDelete("/RemoveProducts", RemoveProducts).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapDelete("/RemoveCouponCode", RemoveCouponCode).RequireAuthorization(AdministratorPolicy);
        group.MapPost("/SendMessage", SendMessage).RequireAuthorization(UserAndAdministratorPolicy);
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

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> ShoppingCartUpsert(ISender sender,
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
        ILogger<int> logger, [FromBody] DeleteProductDto deleteProductDto)
    {
        var command = await sender.Send(new RemoveShoppingCartProductRequest(deleteProductDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Продукт из корзины успешно удален: {deleteProductDto.Id}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления продукта из корзины: {deleteProductDto.Id}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> RemoveProductByUser(ISender sender,
        ILogger<DeleteProductByUserDto> logger, [FromBody] DeleteProductByUserDto deleteProductByUserDto)
    {
        var command = await sender.Send(new DeleteCartProductByUserRequest(deleteProductByUserDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Продукт из корзины успешно удален: {deleteProductByUserDto.UserId}: ProductId: {deleteProductByUserDto.ProductId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления продукта из корзины: {deleteProductByUserDto.UserId}: ProductId: {deleteProductByUserDto.ProductId}");
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

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> RemoveCouponCode(ISender sender,
        ILogger<DeleteCouponDto> logger, [FromBody] DeleteCouponDto deleteCouponDto)
    {
        var command = await sender.Send(new DeleteCouponRequest(deleteCouponDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Купон успешно удален: {deleteCouponDto.CouponCode}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления купона: {deleteCouponDto.CouponCode}");
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