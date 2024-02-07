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
            return Ok(request);
        }

        // POST api/<FavouriteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}