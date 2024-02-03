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

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> GetUserById(string id)
        {
            var command = await _mediator.Send(new GetUserDetailsRequest { Id = id });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Put([FromBody] UpdateUserDto updateUser)
        {
            var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ValidationErrors);
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfile)
        {
            var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ValidationErrors);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<DeleteUserDto>>> Delete([FromBody] DeleteUserDto deleteUserDto)
        {
            var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }

        [HttpDelete()]
        public async Task<ActionResult<Result<bool>>> Delete([FromBody] DeleteUserListDto deleteUserListDto)
        {
            var comamnd = await _mediator.Send(new DeleteUserListRequest { DeleteUserList = deleteUserListDto });

            if (comamnd.IsSuccess)
            {
                return Ok(comamnd);
            }

            return BadRequest(comamnd.ErrorMessage);
        }
    }
}