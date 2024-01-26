﻿using Favourites.Application.DTOs;
using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Favourites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET api/<FavouriteController>/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<FavouriteAPIResponse>> Get(string userId)
        {
            var query = await _mediator.Send(new GetFavouriteProductsRequest { UserId = userId });
            return Ok(query);
        }

        // POST api/<FavouriteController>
        [HttpPost]
        public async Task<ActionResult<FavouriteAPIResponse>> Post([FromBody] FavouritesDto favouritesDto)
        {
            var command = await _mediator.Send(new FavoutiteUpsertRequest { Favourites = favouritesDto });
            return Ok(command);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}