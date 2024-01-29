using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Features.Requests.Queries;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<CollectionResult<ProductDto>>> Get()
        {
            var query = await _mediator.Send(new GetProductListRequest());

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Get(int id)
        {
            var query = await _mediator.Send(new GetProductDetailsRequest { Id = id });

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<Result<ProductDto>>> Post([FromForm] CreateProductDto createProductDto)
        {
            var command = await _mediator.Send(new CreateProductDtoRequest { CreateProduct = createProductDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Put([FromForm] UpdateProductDto updateProductDto)
        {
            var command = await _mediator.Send(new UpdateProductDtoRequest { UpdateProduct = updateProductDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> Delete([FromBody] DeleteProductDto deleteProductDto)
        {
            var command = await _mediator.Send(new DeleteProductDtoRequest { DeleteProduct = deleteProductDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // DELETE api/<ProductController>/
        [HttpDelete]
        public async Task<ActionResult<Result<ProductDto>>> Delete([FromBody] DeleteProductsDto deleteProductsDto)
        {
            var command = await _mediator.Send(new DeleteProductsDtoRequest { DeleteProducts = deleteProductsDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }
    }
}