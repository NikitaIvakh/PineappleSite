using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Utility;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.API.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductController : ControllerBase
{
    [HttpPost("CreateProduct")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<int>>> CreateProduct(ISender sender, ILogger<int> logger,
        [FromForm] CreateProductDto createProductDto)
    {
        var command = await sender.Send(new CreateProductRequest(createProductDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно добавлен: {createProductDto.Name}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка добавления продукта: {createProductDto.Name}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPut("UpdateProduct/{productId:int}")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<Unit>>> UpdateProduct(ISender sender, ILogger<Unit> logger,
        [FromRoute] int productId, [FromForm] UpdateProductDto updateProductDto)
    {
        var command = await sender.Send(new UpdateProductRequest(updateProductDto));

        if (command.IsSuccess && productId == updateProductDto.Id)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно обновлен: {updateProductDto.Id}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка обновления продукта: {updateProductDto.Id}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}