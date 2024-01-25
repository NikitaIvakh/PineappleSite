using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Application.Response;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ShoppingCartController>
        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> GetCart(string userId)
        {
            var cart = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
            return Ok(cart);
        }

        // POST api/<ShoppingCartController>
        [HttpPost("CartUpsert")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> CartUpsert([FromBody] CartDto cartDto)
        {
            var comnand = await _mediator.Send(new CartUpsertRequest { CartDto = cartDto });
            return Ok(comnand);
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ApplyCouponRequest { CartDto = cartDto });
            return Ok(command);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> RemoveCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new RemoveCouponRequest { CartDto = cartDto });
            return Ok(command);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("RemoveCart/{cartDerailsId}")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> RemoveCart([FromBody] int cartDerailsId)
        {
            var command = await _mediator.Send(new RemoveCartRequest { CartDetailsId = cartDerailsId });
            return Ok(command);
        }
    }
}