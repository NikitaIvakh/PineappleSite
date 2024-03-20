using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Utility;
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
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
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
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // POST api/<ProductController>
        [HttpPost]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<ProductDto>>> Post([FromForm] CreateProductDto createProductDto)
        {
            var command = await _mediator.Send(new CreateProductDtoRequest { CreateProduct = createProductDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт успешно добавлен: {createProductDto.Name}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка добавления продукта: {createProductDto.Name}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // PUT api/<ProductController>/5
        [HttpPut("{productId}")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<ProductDto>>> Put(int productId, [FromForm] UpdateProductDto updateProductDto)
        {
            if (productId == updateProductDto.Id)
            {
                var command = await _mediator.Send(new UpdateProductDtoRequest { UpdateProduct = updateProductDto });

                if (command.IsSuccess)
                {
                    _logger.LogDebug($"LogDebug ================ Продукт успешно обновлен: {updateProductDto.Id}");
                    return Ok(command);
                }

                _logger.LogError($"LogDebugError ================ Ошибка обновления продукта: {updateProductDto.Id}");
                foreach (var error in command.ValidationErrors!)
                {
                    return BadRequest(error);
                }

                return NoContent();
            }

            return NoContent();
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{productId}")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<ProductDto>>> Delete(int productId, [FromBody] DeleteProductDto deleteProductDto)
        {
            if (productId == deleteProductDto.Id)
            {
                var command = await _mediator.Send(new DeleteProductDtoRequest { DeleteProduct = deleteProductDto });

                if (command.IsSuccess)
                {
                    _logger.LogDebug($"LogDebug ================ Продукт успешно удален: {deleteProductDto.Id}");
                    return Ok(command);
                }

                _logger.LogError($"LogDebugError ================ Ошибка удаления продукта: {deleteProductDto.Id}");
                foreach (var error in command.ValidationErrors!)
                {
                    return BadRequest(error);
                }

                return NoContent();
            }

            return NoContent();
        }

        // DELETE api/<ProductController>/
        [HttpDelete]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<CollectionResult<ProductDto>>> Delete([FromBody] DeleteProductsDto deleteProductsDto)
        {
            var command = await _mediator.Send(new DeleteProductsDtoRequest { DeleteProducts = deleteProductsDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукты успешно удалены: {deleteProductsDto.ProductIds}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления продуктов: {deleteProductsDto.ProductIds}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}