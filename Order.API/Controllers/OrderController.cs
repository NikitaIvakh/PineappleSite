using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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