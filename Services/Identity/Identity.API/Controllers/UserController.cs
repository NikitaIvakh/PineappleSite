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
public sealed class UserController : ControllerBase
{
    [HttpPut("UpdateUserProfile/{userId}")]
    [Authorize(Policy = StaticDetails.UserAndAdministratorPolicy)]
    public async Task<ActionResult<Result<GetUserForUpdateDto>>> UpdateUserProfile(ISender sender,
        ILogger<GetUserForUpdateDto> logger, [FromRoute] string userId,
        [FromForm] UpdateUserProfileDto updateUserProfileDto)
    {
        var command = await sender.Send(new UpdateUserProfileRequest(updateUserProfileDto));

        if (command.IsSuccess && userId == updateUserProfileDto.Id)
        {
            logger.LogDebug(
                $"LogDebug ================ Профиль пользователя успешно обновлен: {updateUserProfileDto.Id}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Обновить профиль пользователеля не удалось: {updateUserProfileDto.Id}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}