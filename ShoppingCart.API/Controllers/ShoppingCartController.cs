using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ShoppingCartController>
        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<Result<CartDto>>> GetCart(string userId)
        {
            var cart = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
            return Ok(cart);
        }

        // POST api/<ShoppingCartController>
        [HttpPost("CartUpsert")]
        public async Task<ActionResult<Result<CartDto>>> CartUpsert([FromBody] CartDto cartDto)
        {
            var comnand = await _mediator.Send(new CartUpsertRequest { CartDto = cartDto });
            return Ok(comnand);
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<Result<CartDto>>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ApplyCouponRequest { CartDto = cartDto });
            return Ok(command);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ActionResult<Result<CartDto>>> RemoveCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new RemoveCouponRequest { CartDto = cartDto });
            return Ok(command);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("RemoveCart/{cartDerailsId}")]
        public async Task<ActionResult<Result<CartDto>>> RemoveCart([FromBody] int cartDerailsId)
        {
            var command = await _mediator.Send(new RemoveCartRequest { CartDetailsId = cartDerailsId });
            return Ok(command);
        }
    }
}