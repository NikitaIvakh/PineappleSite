using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.DTOs.Products;
using Product.Application.Features.Requests.Queries;

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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}