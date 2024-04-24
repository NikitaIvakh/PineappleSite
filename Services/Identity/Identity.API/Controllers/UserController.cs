using Identity.API.Utility;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UserController(ISender mediator) : ControllerBase
{
    [HttpGet("GetUsers")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<CollectionResult<GetUsersDto>>> GetUsers(ILogger<GetUsersDto> logger)
    {
        var request = await mediator.Send(new GetUsersRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователи успешно получены");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Получить пользователей не удалось");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    [HttpGet("GetUser/{userId}")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<GetUserDto>>> GetUser(ILogger<GetUserDto> logger, [FromRoute] string userId)
    {
        var request = await mediator.Send(new GetUserRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно получен: {userId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения пользователя: {userId}");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }
    
    [HttpGet("GetUserForUpdateProfile/{userId}")]
    [Authorize(Policy = StaticDetails.UserAndAdministratorPolicy)]
    public async Task<ActionResult<Result<GetUserForUpdateDto>>> GetUserForUpdateProfile(
        ILogger<GetUserForUpdateDto> logger, [FromRoute] string userId, string? password)
    {
        var request = await mediator.Send(new GetUserForUpdateRequest(userId, password));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Профиль пользователя успешно получен: {userId}");
            return Ok(request);
        }

        logger.LogError($"LogDebugError ================ Получить профиль пользователя не удалось: {userId}");
        return BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    [HttpPost("CreateUser")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<Unit>>> CreateUser(ILogger<CreateUserDto> logger,
        [FromBody] CreateUserDto createUserDto)
    {
        var command = await mediator.Send(new CreateUserRequest(createUserDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно создан: {createUserDto.UserName}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Удалить пользователея не удалось: {createUserDto.UserName}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPut("UpdateUser/{useId}")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<Unit>>> UpdateUser(ILogger<UpdateUserDto> logger, [FromRoute] string useId,
        [FromBody] UpdateUserDto updateUserDto)
    {
        var command = await mediator.Send(new UpdateUserRequest(updateUserDto));

        if (command.IsSuccess && useId == updateUserDto.Id)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно обновлен: {updateUserDto.Id}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Обновить пользователеля не удалось: {updateUserDto.Id}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPut("UpdateUserProfile/{userId}")]
    [Authorize(Policy = StaticDetails.UserAndAdministratorPolicy)]
    public async Task<ActionResult<Result<GetUserForUpdateDto>>> UpdateUserProfile(ILogger<GetUserForUpdateDto> logger,
        [FromRoute] string userId, [FromForm] UpdateUserProfileDto updateUserProfileDto)
    {
        var command = await mediator.Send(new UpdateUserProfileRequest(updateUserProfileDto));

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

    [HttpDelete("DeleteUser/{userId}")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<Result<Unit>>> DeleteUser(ILogger<DeleteUserDto> logger, [FromRoute] string userId,
        [FromBody] DeleteUserDto deleteUserDto)
    {
        var command = await mediator.Send(new DeleteUserRequest(deleteUserDto));

        if (command.IsSuccess && userId == deleteUserDto.UserId)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно удален: {deleteUserDto.UserId}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Удалить пользователеля не удалось: {deleteUserDto.UserId}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpDelete("DeleteUsers")]
    [Authorize(Policy = StaticDetails.AdministratorPolicy)]
    public async Task<ActionResult<CollectionResult<Unit>>> DeleteUsers(ILogger<DeleteUsersDto> logger,
        [FromBody] DeleteUsersDto deleteUsersDto)
    {
        var command = await mediator.Send(new DeleteUsersRequest(deleteUsersDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователи успешно удалены: {deleteUsersDto.UserIds}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Удалить пользователелей не удалось: {deleteUsersDto.UserIds}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}