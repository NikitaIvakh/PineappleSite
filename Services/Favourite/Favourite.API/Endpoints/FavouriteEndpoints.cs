using Carter;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Favourite.API.Utility.StaticDetails;

namespace Favourite.API.Endpoints;

public sealed class FavouriteEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/favourites").RequireAuthorization(UserAndAdministratorPolicy);

        group.MapGet("/GetFavouriteProducts/{userId}", GetFavouriteProducts);
        group.MapPost("/FavouriteUpsertAsync", FavouriteUpsertAsync);
        group.MapDelete("/DeleteFavouriteProduct/{productId:int}", DeleteFavouriteProduct);
        group.MapDelete("/DeleteFavouriteProducts", DeleteFavouriteProducts);
    }

    private static async Task<Results<Ok<Result<FavouriteDto>>, BadRequest<string>>> GetFavouriteProducts(string userId,
        ISender sender, ILogger<FavouriteDto> logger)
    {
        var request = await sender.Send(new GetFavouriteProductsRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Избранные товары успешно получены: {userId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения избранных товаров: {userId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<FavouriteHeaderDto>>, BadRequest<string>>> FavouriteUpsertAsync(
        ISender sender, ILogger<FavouriteHeaderDto> logger, [FromBody] FavouriteDto favouriteDto)
    {
        var command = await sender.Send(new FavouriteProductUpsertRequest(favouriteDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Избранные товары успешно обновлены: {favouriteDto.FavouriteHeader.FavouriteHeaderId}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка обновления избранного товара: {favouriteDto.FavouriteHeader.FavouriteHeaderId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteFavouriteProduct(ISender sender,
        ILogger<FavouriteHeaderDto> logger, int productId)
    {
        var command = await sender.Send(new DeleteFavouriteProductRequest(productId));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Избранный товар успешно удален: {productId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления избранного товара: {productId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<FavouriteHeaderDto>>, BadRequest<string>>> DeleteFavouriteProducts(
        ISender sender, ILogger<FavouriteHeaderDto> logger,
        [FromBody] DeleteFavouriteProductsDto deleteFavouriteProductsDto)
    {
        var command = await sender.Send(new DeleteFavouriteProductsRequest(deleteFavouriteProductsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Избранные товары успешно удалены: {deleteFavouriteProductsDto.ProductIds}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления избранных товаров: {deleteFavouriteProductsDto.ProductIds}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}