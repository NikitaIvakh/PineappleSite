using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Favourite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET api/<FavouriteController>/5
        [HttpGet("GetFavouriteProducts/{userId}")]
        public async Task<ActionResult<Result<FavouriteDto>>> GetFavouriteProducts(string userId)
        {
            var request = await _mediator.Send(new GetFavouriteFroductsRequest { UserId = userId });

            if (request.IsSuccess)
            {
                return Ok(request);
            }

            return BadRequest(request.ErrorMessage);
        }

        // POST api/<FavouriteController>
        [HttpPost("FavouriteUpsertAsync")]
        public async Task<ActionResult<Result<FavouriteDto>>> FavouriteUpsertAsync([FromBody] FavouriteDto favouriteDto)
        {
            var command = await _mediator.Send(new FavouriteProductUpsertRequest { FavouriteDto = favouriteDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{productId}")]
        public async Task<ActionResult<Result<FavouriteDto>>> Delete(int productId)
        {
            var command = await _mediator.Send(new RemoveFavouriteProductRequest { ProductId = productId });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }
    }
}