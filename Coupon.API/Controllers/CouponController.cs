﻿using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Requests.Commands;
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
        public async Task<ActionResult<CouponDto>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest { CreateCouponDto = createCouponDto });
            return Ok(command);
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CouponDto>> Put([FromBody] UpdateCouponDto updateCouponDto)
        {
            var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });
            return Ok(command);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CouponDto>> Delete([FromBody] DeleteCouponDto deleteCouponDto)
        {
            var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });
            return Ok(command);
        }
    }
}