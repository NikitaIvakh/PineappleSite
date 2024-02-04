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
    public class ShoppingCartController(IMediator mediator, ILogger<CartHeaderDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CartHeaderDto> _logger = logger;

        // GET: api/<ShoppingCartController>
        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<Result<CartDto>>> GetCart(string userId)
        {
            var cart = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });

            if (cart.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Корзина успешно получена");
                return Ok(cart);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения корзины");
            return BadRequest(cart.ErrorMessage);
        }

        // POST api/<ShoppingCartController>
        [HttpPost("CartUpsert")]
        public async Task<ActionResult<Result<CartDto>>> CartUpsert([FromBody] CartDto cartDto)
        {
            var comnand = await _mediator.Send(new CartUpsertRequest { CartDto = cartDto });

            if (comnand.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Корзина успешно обновлена");
                return Ok(comnand);
            }

            _logger.LogError("LogDebugError ================ Ошибка обновления корзины");
            return BadRequest(comnand.ErrorMessage);
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<Result<CartDto>>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ApplyCouponRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно применен");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка применения купона");
            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ActionResult<Result<CartDto>>> RemoveCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new RemoveCouponRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно удален");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления купона");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("RemoveCart/{cartDerailsId}")]
        public async Task<ActionResult<Result<CartDto>>> RemoveCart([FromBody] int cartDerailsId)
        {
            var command = await _mediator.Send(new RemoveCartRequest { CartDetailsId = cartDerailsId });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Продукт успешно удален");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления продукта");
            return BadRequest(command.ErrorMessage);
        }
    }
}