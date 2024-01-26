using Favourites.Application.DTOs;
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
        [HttpGet("GetFavourite/{userId}")]
        public async Task<ActionResult<FavouriteAPIResponse>> GetFavourite(string userId)
        {
            var query = await _mediator.Send(new GetFavouriteProductsRequest { UserId = userId });
            return Ok(query);
        }

        // POST api/<FavouriteController>
        [HttpPost("FavouriteUpsert")]
        public async Task<ActionResult<FavouriteAPIResponse>> FavouriteUpsert([FromBody] FavouritesDto favouritesDto)
        {
            var command = await _mediator.Send(new FavoutiteUpsertRequest { Favourites = favouritesDto });
            return Ok(command);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("RemoveDetails/{favouriteDetailsId}")]
        public async Task<ActionResult<FavouriteAPIResponse>> RemoveDetails([FromBody] int favouriteDetailsId)
        {
            var command = await _mediator.Send(new RemoveFavoriteRequest { FavouriteDetailId = favouriteDetailsId });
            return Ok(command);
        }
    }
}