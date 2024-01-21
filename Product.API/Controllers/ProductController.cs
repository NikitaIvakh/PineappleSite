using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Features.Requests.Queries;
using Product.Application.Response;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ProductDto>>> Get()
        {
            var query = await _mediator.Send(new GetProductListRequest());
            return Ok(query);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var query = await _mediator.Send(new GetProductDetailsRequest { Id = id });
            return Ok(query);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<ActionResult<ProductAPIResponse>> Post([FromBody] CreateProductDto createProductDto)
        {
            var command = await _mediator.Send(new CreateProductDtoRequest { CreateProduct = createProductDto });
            return Ok(command);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put([FromBody] UpdateProductDto updateProductDto)
        {
            var command = await _mediator.Send(new UpdateProductDtoRequest { UpdateProduct = updateProductDto });
            return Ok(command);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete([FromBody] DeleteProductDto deleteProductDto)
        {
            var command = await _mediator.Send(new DeleteProductDtoRequest { DeleteProduct = deleteProductDto });
            return Ok(command);
        }
    }
}