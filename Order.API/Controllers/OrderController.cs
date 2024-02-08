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
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<OrderController>
        [HttpGet("GetAllOrders/{userId}")]
        public async Task<ActionResult<CollectionResult<OrderHeaderDto>>> GetAllOrders(string userId)
        {
            var request = await _mediator.Send(new GetOrderListRequest { UserId = userId });
            return Ok(request);
        }

        // GET api/<OrderController>/5
        [HttpGet("GetOrder/{orderId}")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> GetOrder(int orderId)
        {
            var request = await _mediator.Send(new GetOrderRequest { OrderId = orderId });
            return Ok(request);
        }

        // POST api/<OrderController>
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> CreateOrder([FromBody] CartDto cartDto)
        {
            var command = await _mediator.Send(new CreateOrderRequest { CartDto = cartDto });
            return Ok(command);
        }

        // POST api/<OrderController>
        [HttpPost("CreateStripeSession")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> CreateStripeSession([FromBody] StripeRequestDto stripeRequestDto)
        {
            var command = await _mediator.Send(new CreateStripeRequest { StripeRequest = stripeRequestDto });
            return Ok(command);
        }

        // POST api/<OrderController>
        [HttpPost("ValidateStripeSession")]
        public async Task<ActionResult<Result<OrderHeaderDto>>> ValidateStripeSession([FromBody] int orderHeaderId)
        {
            var command = await _mediator.Send(new ValidateStripeSessionRequest { OrderHeaderId = orderHeaderId });
            return Ok(command);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}