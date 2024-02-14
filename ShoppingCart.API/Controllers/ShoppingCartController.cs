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
                _logger.LogDebug($"LogDebug ================ Корзина успешно получена: {userId}");
                return Ok(request);
            }

            _logger.LogError($"LogDebugError ================ Ошибка получения корзины: {userId}");
            return BadRequest(request.ErrorMessage);
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public async Task<ActionResult<Result<CartDto>>> Post([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ShoppingCartUpsertRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ В корзину успешно добавлен товар: {cartDto.CartHeader.CartHeaderId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка добавления товара в корзину: {cartDto.CartHeader.CartHeaderId}");
            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<Result<CartDto>>> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new ApplyCouponRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно применен: {cartDto.CartHeader.CartHeaderId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка применения купона: {cartDto.CartHeader.CartHeaderId}");
            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ActionResult<Result<CartDto>>> RemoveCoupon([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new RemoveCouponRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно удален: {cartDto.CartHeader.CartHeaderId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления купона: {cartDto.CartHeader.CartHeaderId}");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{productId}")]
        public async Task<ActionResult<Result<CartDto>>> RemoveDetails([FromBody] int productId)
        {
            var command = await _mediator.Send(new RemoveShoppingCartDetailsRequest { ProductId = productId });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Продукт из корзины успешно удален: {productId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления продукта из корзины: {productId}");
            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("RabbitMQShoppingCartRequest")]
        public async Task<ActionResult<Result<bool>>> RabbitMQShoppingCartRequest([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new RabbitMQSendRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Письмо в очередь успешно доставлено: {cartDto.CartHeader.CartHeaderId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка добавления письма в очередь: {cartDto.CartHeader.CartHeaderId}");
            return BadRequest(command.ErrorMessage);
        }
    }
}