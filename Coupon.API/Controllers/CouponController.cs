using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Coupon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<CouponController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CouponDto>>> Get()
        {
            var query = await _mediator.Send(new GetCouponListRequest());
            return Ok(query);
        }

        // GET api/<CouponController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CouponDto>> Get(int id)
        {
            var query = await _mediator.Send(new GetCouponDetailsRequest() { Id = id });
            return Ok(query);
        }

        // POST api/<CouponController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}