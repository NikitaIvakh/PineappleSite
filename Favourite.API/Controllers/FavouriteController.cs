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
    public class FavouriteController(IMediator mediator, ILogger<FavouriteDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<FavouriteDto> _logger = logger;

        // GET api/<FavouriteController>/5
        [HttpGet("GetFavouriteProducts/{userId}")]
        public async Task<ActionResult<Result<FavouriteDto>>> GetFavouriteProducts(string userId)
        {
            var request = await _mediator.Send(new GetFavouriteFroductsRequest { UserId = userId });

            if (request.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранные товары успешно получены");
                return Ok(request);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения избранных товаров");
            return BadRequest(request.ErrorMessage);
        }

        // POST api/<FavouriteController>
        [HttpPost("FavouriteUpsertAsync")]
        public async Task<ActionResult<Result<FavouriteDto>>> FavouriteUpsertAsync([FromBody] FavouriteDto favouriteDto)
        {
            var command = await _mediator.Send(new FavouriteProductUpsertRequest { FavouriteDto = favouriteDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранны1 товарр успешно обновден");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка обновления избранного товара");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{productId}")]
        public async Task<ActionResult<Result<FavouriteDto>>> Delete(int productId)
        {
            var command = await _mediator.Send(new RemoveFavouriteProductRequest { ProductId = productId });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Избранный товар успешно удален");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления избранного товара");
            return BadRequest(command.ErrorMessage);
        }
    }
}