using Favourites.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ShoppingCartController>
        [HttpGet("GetShoppingCart/{userId}")]
        public async Task<ActionResult<Result<CartDto>>> GetShoppingCart(string userId)
        {
            var request = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
            return Ok(request);
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public async Task<ActionResult<Result<CartDto>>> Post([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ShoppingCartUpsertRequest { CartDto = cartDto });
            return Ok(command);
        }

        // PUT api/<ShoppingCartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}