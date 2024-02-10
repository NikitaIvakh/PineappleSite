using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Requests.Commands;
using Order.Application.Features.Requests.Requests;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.ResultOrder;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<OrderController>
        [HttpGet("GetAllOrders/{userId}")]
        public async Task<ActionResult<CollectionResult<OrderHeaderDto>>> GetAllOrders(string userId)
        {
            var request = await _mediator.Send(new GetOrderListRequest { UserId = userId });

            if (request.IsSuccess)
            {
                return Ok(request);
            }

            return BadRequest(request.ErrorMessage);
        }

        // GET api/<OrderController>/5
        [HttpGet("GetOrder/{orderId}")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> GetOrder(int orderId)
        {
            var request = await _mediator.Send(new GetOrderRequest { OrderId = orderId });

            if (request.IsSuccess)
            {
                return Ok(request);
            }

            return BadRequest(request.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> CreateOrder([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new CreateOrderRequest { CartDto = cartDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<Result<StripeRequestDto>>> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            var command = await _mediator.Send(new CreateStripeRequest { StripeRequest = stripeRequestDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            var command = await _mediator.Send(new ValidateStripeSessionRequest { OrderHeaderId = orderHeaderId });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // POST api/<OrderController>
        [HttpPost("UpdateOrderStatus")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> UpdateOrderStatus(int orderHeaderId, [FromBody] string newStatus)
        {
            var command = await _mediator.Send(new UpdateOrderStatusRequest { OrderHeaderId = orderHeaderId, NewStatus = newStatus });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }
    }
}