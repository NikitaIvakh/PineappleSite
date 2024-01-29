using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;
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
        public async Task<ActionResult<Result<FavouritesDto>>> GetFavourite(string userId)
        {
            var query = await _mediator.Send(new GetFavouriteProductsRequest { UserId = userId });

            if (query.IsSuccess)
            {
                return Ok(query);
            }

            return BadRequest(query.ErrorMessage);
        }

        // POST api/<FavouriteController>
        [HttpPost("FavouriteUpsert")]
        public async Task<ActionResult<Result<FavouritesDto>>> FavouriteUpsert([FromBody] FavouritesDto favouritesDto)
        {
            var command = await _mediator.Send(new FavoutiteUpsertRequest { Favourites = favouritesDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("RemoveFavouriteDetails/{favouriteDetailsId}")]
        public async Task<ActionResult<Result<FavouritesDto>>> RemoveFavouriteDetails([FromBody] int favouriteDetailsId)
        {
            var command = await _mediator.Send(new RemoveFavoriteRequest { FavouriteDetailId = favouriteDetailsId });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }
    }
}