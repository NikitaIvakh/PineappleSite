using Identity.Application.DTOs.Authentications;
using Identity.Application.DTOs.Identities;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Application.Response;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static Identity.Application.Utilities.StaticDetails;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserWithRolesDto>>> GetAllUsers(string userId = "")
        {
            var command = await _mediator.Send(new GetUserListRequest() { UserId = userId });
            return Ok(command);
        }

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<UserWithRolesDto>> GetUserById(string id)
        {
            var command = await _mediator.Send(new GetUserDetailsRequest { Id = id });
            return Ok(command);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BaseIdentityResponse<RegisterResponseDto>>> Put([FromBody] UpdateUserDto updateUser)
        {
            var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });
            return Ok(command);
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<ActionResult<BaseIdentityResponse<UserWithRoles>>> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfile)
        {
            var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });
            return Ok(command);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseIdentityResponse<DeleteUserDto>>> Delete([FromBody] DeleteUserDto deleteUserDto)
        {
            var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });
            return Ok(command);
        }
    }
}