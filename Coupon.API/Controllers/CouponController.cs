using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
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
        [HttpGet("Coupons")]
        public async Task<ActionResult<CollectionResult<CouponDto>>> GetCoupons()
        {
            var query = await _mediator.Send(new GetCouponListRequest());

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query.ErrorCode);
        }

        // GET api/<CouponController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> GetCoupon(int id)
        {
            var query = await _mediator.Send(new GetCouponDetailsRequest() { Id = id });

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query);
        }

        [HttpGet("GetCouponByCode/{couponCode}")]
        public async Task<ActionResult<Result<CouponDto>>> GetCouponByCode(string couponCode)
        {
            var query = await _mediator.Send(new GetCouponDetailsByCouponNameRequest { CouponCode = couponCode });

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query);
        }

        // POST api/<CouponController>
        [HttpPost]
        public async Task<ActionResult<Result<CouponDto>>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest { CreateCoupon = createCouponDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Put([FromBody] UpdateCouponDto updateCouponDto)
        {
            var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Delete([FromBody] DeleteCouponDto deleteCouponDto)
        {
            var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete]
        public async Task<ActionResult<Result<CouponDto>>> Delete([FromBody] DeleteCouponListDto deleteCouponListDto)
        {
            var command = await _mediator.Send(new DeleteCouponListRequest { DeleteCoupon = deleteCouponListDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command);
        }
    }
}