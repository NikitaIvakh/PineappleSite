using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController(IMediator mediator, ILogger<CartDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CartDto> _logger = logger;

        // GET: api/<ShoppingCartController>
        [HttpGet("GetShoppingCart/{userId}")]
        public async Task<ActionResult<Result<CartDto>>> GetShoppingCart(string userId)
        {
            var request = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });

            if (request.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Корзина успешно получена");
                return Ok(request);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения корзины");
            return BadRequest(request.ErrorMessage);
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public async Task<ActionResult<Result<CartDto>>> Post([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ShoppingCartUpsertRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ В корзину успешно добавлен товар");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка добавления товара в корзину");
            return BadRequest(command.ErrorMessage);
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
        [HttpDelete("{cartDetailsId}")]
        public async Task<ActionResult<Result<CartDto>>> RemoveDetails(int cartDetailsId)
        {
            var command = await _mediator.Send(new RemoveShoppingCartDetailsRequest { CartDetailsId = cartDetailsId });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Продукт из корзины успешно удален");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления продукта из корзины");
            return BadRequest(command.ErrorMessage);
        }
    }
}