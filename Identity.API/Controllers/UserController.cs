using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
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
        public async Task<ActionResult<CollectionResult<UserWithRolesDto>>> GetAllUsers(string userId = "")
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
        public async Task<ActionResult<Result<RegisterResponseDto>>> Put([FromBody] UpdateUserDto updateUser)
        {
            var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });
            return Ok(command);
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfile)
        {
            var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });
            return Ok(command);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<DeleteUserDto>>> Delete([FromBody] DeleteUserDto deleteUserDto)
        {
            var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });
            return Ok(command);
        }

        [HttpDelete()]
        public async Task<ActionResult<Result<DeleteUserListDto>>> Delete([FromBody] DeleteUserListDto deleteUserListDto)
        {
            var comamnd = await _mediator.Send(new DeleteUserListRequest { DeleteUserList = deleteUserListDto });
            return Ok(comamnd);
        }
    }
}