﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Features.Requests.Queries;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IMediator mediator, ILogger<ProductDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ProductDto> _logger = logger;

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<CollectionResult<ProductDto>>> Get()
        {
            var query = await _mediator.Send(new GetProductListRequest());

            if (query.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Продукты успешно получены");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения продуктов");
            return BadRequest(query);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Get(int id)
        {
            var query = await _mediator.Send(new GetProductDetailsRequest { Id = id });

            if (query.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт успешно получен: {id}");
                return Ok(query);
            }

            _logger.LogError($"LogDebugError ================ Ошибка получения продукта: {id}");
            return BadRequest(query.ErrorMessage);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Result<ProductDto>>> Post([FromForm] CreateProductDto createProductDto)
        {
            var command = await _mediator.Send(new CreateProductDtoRequest { CreateProduct = createProductDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт успешно добавлен: {createProductDto.Name}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка добавления продукта: {createProductDto.Name}");
            return BadRequest(command.ErrorMessage);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Put([FromForm] UpdateProductDto updateProductDto)
        {
            var command = await _mediator.Send(new UpdateProductDtoRequest { UpdateProduct = updateProductDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт успешно обновлен: {updateProductDto.Id}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка обновления продукта: {updateProductDto.Id}");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Delete([FromBody] DeleteProductDto deleteProductDto)
        {
            var command = await _mediator.Send(new DeleteProductDtoRequest { DeleteProduct = deleteProductDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт успешно удален: {deleteProductDto.Id}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления продукта: {deleteProductDto.Id}");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<ProductController>/
        [HttpDelete]
        public async Task<ActionResult<CollectionResult<ProductDto>>> Delete([FromBody] DeleteProductsDto deleteProductsDto)
        {
            var command = await _mediator.Send(new DeleteProductsDtoRequest { DeleteProducts = deleteProductsDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукты успешно удалены: {deleteProductsDto.ProductIds}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления продуктов: {deleteProductsDto.ProductIds}");
            return BadRequest(command.ErrorMessage);
        }
    }
}