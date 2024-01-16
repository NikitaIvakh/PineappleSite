using Identity.Application.DTOs.Identities;
using Identity.Application.Features.Identities.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsers()
        {
            var command = await _mediator.Send(new GetUserListRequest());
            return Ok(command);
        }

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<UserWithRolesDto>> GetUserById(string id)
        {
            var command = await _mediator.Send(new GetUserDetailsRequest { Id = id });
            return Ok(command);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
