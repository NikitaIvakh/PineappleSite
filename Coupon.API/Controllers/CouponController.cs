using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Response;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CouponDto>>> Get()
        {
            var query = await _mediator.Send(new GetCouponListRequest());
            return Ok(query);
        }

        // GET api/<CouponController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CouponDto>> Get(int id)
        {
            var query = await _mediator.Send(new GetCouponDetailsRequest() { Id = id });
            return Ok(query);
        }

        // POST api/<CouponController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest { CreateCouponDto = createCouponDto });
            return Ok(command);
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> Put([FromBody] UpdateCouponDto updateCouponDto)
        {
            var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });
            return Ok(command);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> Delete([FromBody] DeleteCouponDto deleteCouponDto)
        {
            var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });
            return Ok(command);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseCommandResponse>> Delete([FromBody] DeleteCouponListDto deleteCouponListDto)
        {
            var command = await _mediator.Send(new DeleteCouponListRequest { DeleteCoupon = deleteCouponListDto });
            return Ok(command);
        }
    }
}