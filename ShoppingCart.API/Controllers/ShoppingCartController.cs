﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Features.Requests.Handlers;
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
        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<ShoppingCartAPIResponse>> GetCart(string userId)
        {
            var cart = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
            return Ok(cart);
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CartDto cartDto)
        {
            var comnand = await _mediator.Send(new CartUpsertRequest { CartDto = cartDto });
            return Ok(comnand);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}