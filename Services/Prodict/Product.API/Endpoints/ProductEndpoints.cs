using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Features.Requests.Queries;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;
using static Product.API.Utility.StaticDetails;

namespace Product.API.Endpoints;

public sealed class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products");

        group.MapGet("/GetProducts", GetProducts);
        group.MapGet("/GetProduct/{productId:int}", GetProduct).RequireAuthorization(UserAndAdministratorPolicy);
        group.MapDelete("/DeleteProduct/{productId:int}", DeleteProduct).RequireAuthorization(AdministratorPolicy);
        group.MapDelete("/DeleteProducts", DeleteProducts).RequireAuthorization(AdministratorPolicy);
    }

    private static async Task<Results<Ok<CollectionResult<ProductDto>>, BadRequest<string>>> GetProducts(ISender sender,
        ILogger<ProductDto> logger)
    {
        var request = await sender.Send(new GetProductsRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug("LogDebug ================ Продукты успешно получены");
            return TypedResults.Ok(request);
        }

        logger.LogError("LogDebugError ================ Ошибка получения продуктов");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<ProductDto>>, BadRequest<string>>> GetProduct(ISender sender,
        ILogger<ProductDto> logger, [FromRoute] int productId)
    {
        var request = await sender.Send(new GetProductRequest(productId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно получен: {productId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения продукта: {productId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteProduct(ISender sender,
        ILogger<int> logger, [FromRoute] int productId, [FromBody] DeleteProductDto deleteProductDto)
    {
        var command = await sender.Send(new DeleteProductRequest(deleteProductDto));

        if (command.IsSuccess && productId == deleteProductDto.Id)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно удален: {deleteProductDto.Id}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления продукта: {deleteProductDto.Id}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<CollectionResult<bool>>, BadRequest<string>>> DeleteProducts(ISender sender,
        ILogger<Unit> logger, [FromBody] DeleteProductsDto deleteProductsDto)
    {
        var command = await sender.Send(new DeleteProductsRequest(deleteProductsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукты успешно удалены: {deleteProductsDto.ProductIds}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления продуктов: {deleteProductsDto.ProductIds}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}