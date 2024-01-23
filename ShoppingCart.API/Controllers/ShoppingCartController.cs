﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Application.Response;

namespace ShoppingCart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<ShoppingCartController>
        [HttpGet("{userId}")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> Get(string userId)
        {
            var cart = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
            return Ok(cart);
        }

        //// GET api/<ShoppingCartController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ShoppingCartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShoppingCartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}