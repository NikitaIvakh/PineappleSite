using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Requests.Commands;
using Order.Application.Features.Requests.Requests;
using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IMediator mediator, ILogger<OrderHeaderDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<OrderHeaderDto> _logger = logger;

        // GET: api/<OrderController>
        [HttpGet("GetAllOrders/{userId}")]
        public async Task<ActionResult<CollectionResult<OrderHeaderDto>>> GetAllOrders(string userId)
        {
            var request = await _mediator.Send(new GetOrderListRequest { UserId = userId });

            if (request.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Заказы успешно получены");
                return Ok(request);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения заказов");
            return BadRequest(request.ErrorMessage);
        }

        // GET api/<OrderController>/5
        [HttpGet("GetOrder/{orderId}")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> GetOrder(int orderId)
        {
            var request = await _mediator.Send(new GetOrderRequest { OrderId = orderId });

            if (request.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Заказ успешно получен");
                return Ok(request);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения заказа");
            return BadRequest(request.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> CreateOrder([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new CreateOrderRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Заказ успешно создан");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления заказа");
            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<Result<StripeRequestDto>>> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            var command = await _mediator.Send(new CreateStripeRequest { StripeRequest = stripeRequestDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Сессия для оплаты успешно создана");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка создания сессии для оплаты");
            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            var command = await _mediator.Send(new ValidateStripeSessionRequest { OrderHeaderId = orderHeaderId });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Оплата успешно прошла валидацию");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Оплата не успешно прошла валидацию");
            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("UpdateOrderStatus")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> UpdateOrderStatus(int orderHeaderId, [FromBody] string newStatus)
        {
            var command = await _mediator.Send(new UpdateOrderStatusRequest { OrderHeaderId = orderHeaderId, NewStatus = newStatus });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Статус заказ успешно обновлен");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка обновления статуса заказа");
            return BadRequest(command.ErrorMessage);
        }
    }
}