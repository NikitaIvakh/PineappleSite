using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Utility;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Features.Requests.Queries;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductController : ControllerBase
{
    [HttpGet("GetProducts")]
    public async Task<ActionResult<CollectionResult<ProductDto>>> GetProducts(ISender sender,
        ILogger<ProductDto> logger)
    {
        var request = await sender.Send(new GetProductsRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug("LogDebug ================ Продукты успешно получены");
            return Ok(request);
        }

        logger.LogError("LogDebugError ================ Ошибка получения продуктов");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }
    
    [HttpGet("GetProduct/{productId:int}")]
    [Authorize(Policy = StaticDetails.UserAndAdministratorPolicy)]
    public async Task<ActionResult<Result<ProductDto>>> GetProduct(ISender sender, ILogger<ProductDto> logger,
        [FromRoute] int productId)
    {
        var request = await sender.Send(new GetProductRequest(productId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно получен: {productId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения продукта: {productId}");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }

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

    [HttpDelete("DeleteProduct/{productId:int}")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<Unit>>> DeleteProduct(ISender sender, ILogger<int> logger, [FromRoute] int productId,
        [FromBody] DeleteProductDto deleteProductDto)
    {
        var command = await sender.Send(new DeleteProductRequest(deleteProductDto));

        if (command.IsSuccess && productId == deleteProductDto.Id)
        {
            logger.LogDebug($"LogDebug ================ Продукт успешно удален: {deleteProductDto.Id}");
            return Ok(command);
        }
        
        logger.LogError($"LogDebugError ================ Ошибка удаления продукта: {deleteProductDto.Id}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    // DELETE api/<ProductController>/
    [HttpDelete("DeleteProducts")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<CollectionResult<bool>>> DeleteProducts(ISender sender, ILogger<Unit> logger,
        [FromBody] DeleteProductsDto deleteProductsDto)
    {
        var command = await sender.Send(new DeleteProductsRequest(deleteProductsDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Продукты успешно удалены: {deleteProductsDto.ProductIds}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка удаления продуктов: {deleteProductsDto.ProductIds}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}