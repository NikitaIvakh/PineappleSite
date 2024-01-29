using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Favourites.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController(IMediator mediator, ILogger<FavouritesDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<FavouritesDto> _logger = logger;

        // GET api/<FavouriteController>/5
        [HttpGet("GetFavourite/{userId}")]
        public async Task<ActionResult<Result<FavouritesDto>>> GetFavourite(string userId)
        {
            var query = await _mediator.Send(new GetFavouriteProductsRequest { UserId = userId });

            if (query.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранные товары получены");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения избранных товаров");
            return BadRequest(query.ErrorMessage);
        }

        // POST api/<FavouriteController>
        [HttpPost("FavouriteUpsert")]
        public async Task<ActionResult<Result<FavouritesDto>>> FavouriteUpsert([FromBody] FavouritesDto favouritesDto)
        {
            var command = await _mediator.Send(new FavoutiteUpsertRequest { Favourites = favouritesDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранные товары обновлены");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка обновления избранных товаров");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("RemoveFavouriteDetails/{favouriteDetailsId}")]
        public async Task<ActionResult<Result<FavouritesDto>>> RemoveFavouriteDetails([FromBody] int favouriteDetailsId)
        {
            var command = await _mediator.Send(new RemoveFavoriteRequest { FavouriteDetailId = favouriteDetailsId });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранные товары удалены");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления избранных товаров");
            return BadRequest(command.ErrorMessage);
        }
    }
}