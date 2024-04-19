using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Favourite.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FavouriteController(ISender mediator, ILogger<FavouriteDto> logger) : ControllerBase
{
    // GET api/<FavouriteController>/5
    [HttpGet("GetFavouriteProducts/{userId}")]
    public async Task<ActionResult<Result<FavouriteDto>>> GetFavouriteProducts(string userId)
    {
        var request = await mediator.Send(new GetFavouriteProductsRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Избранные товары успешно получены: {userId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения избранных товаров: {userId}");

        foreach (var error in request.ValidationErrors!)
        {
            return BadRequest(error);
        }

        return NoContent();
    }

    // POST api/<FavouriteController>
    [HttpPost("FavouriteUpsertAsync")]
    public async Task<ActionResult<Result<FavouriteDto>>> FavouriteUpsertAsync([FromBody] FavouriteDto favouriteDto)
    {
        var command = await mediator.Send(new FavouriteProductUpsertRequest(favouriteDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Избранны1 товарр успешно обновден: {favouriteDto.FavouriteHeader.FavouriteHeaderId}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка обновления избранного товара: {favouriteDto.FavouriteHeader.FavouriteHeaderId}");
        foreach (var error in command.ValidationErrors!)
        {
            return BadRequest(error);
        }

        return NoContent();
    }

    // DELETE api/<FavouriteController>/5
    [HttpDelete("{productId}")]
    public async Task<ActionResult<Result<FavouriteDto>>> Delete(int productId)
    {
        var command = await mediator.Send(new DeleteFavouriteProductRequest(productId));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Избранный товар успешно удален: {productId}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления избранного товара: {productId}");
        foreach (var error in command.ValidationErrors!)
        {
            return BadRequest(error);
        }

        return NoContent();
    }

    [HttpDelete("DeleteProductList")]
    public async Task<ActionResult<Result<FavouriteDto>>> DeleteProductList(
        [FromBody] DeleteFavouriteProductsDto deleteFavouriteProductsDto)
    {
        var command = await mediator.Send(new DeleteFavouriteProductsRequest(deleteFavouriteProductsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug(
                $"LogDebug ================ Избранные товары успешно удалены: {deleteFavouriteProductsDto.ProductIds}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Ошибка удаления избранных товаров: {deleteFavouriteProductsDto.ProductIds}");
        foreach (var error in command.ValidationErrors!)
        {
            return BadRequest(error);
        }

        return NoContent();
    }
}