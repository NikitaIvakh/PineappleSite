using Identity.API.Utility;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UserController(ISender mediator) : ControllerBase
{
    [HttpPut("UpdateUserProfile/{userId}")]
    [Authorize(Policy = StaticDetails.UserAndAdministratorPolicy)]
    public async Task<ActionResult<Result<GetUserForUpdateDto>>> UpdateUserProfile(
        [FromRoute] string userId, [FromForm] UpdateUserProfileDto updateUserProfileDto)
    {
        var command = await mediator.Send(new UpdateUserProfileRequest(updateUserProfileDto));

        if (command.IsSuccess && userId == updateUserProfileDto.Id)
        {
            return Ok(command);
        }

        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}