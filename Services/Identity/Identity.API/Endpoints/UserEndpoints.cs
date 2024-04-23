using Carter;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Endpoints;

public sealed class UserEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/users");

        group.MapGet("/GetUsers", GetUsers).WithName(nameof(GetUsers));
        group.MapGet("/GetUser/{userId}", GetUser).WithName(nameof(GetUser));
        group.MapPost("/CreateUser", CreateUser).WithName(nameof(CreateUser));
        group.MapPut("/UpdateUser/{userId}", UpdateUser).WithName(nameof(UpdateUser));
        group.Map("/UpdateUserProfile/{userId}", UpdateUserProfile).WithName(nameof(UpdateUserProfile));
        group.MapDelete("/DeleteUser/{userId}", DeleteUser).WithName(nameof(DeleteUser));
    }

    private static async Task<Results<Ok<CollectionResult<GetUsersDto>>, BadRequest<string>>> GetUsers(ISender sender,
        ILogger<GetUsersDto> logger)
    {
        var request = await sender.Send(new GetUsersRequest());

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователи успешно получены");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Получить пользователей не удалось");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<GetUserDto>>, BadRequest<string>>> GetUser(ISender sender,
        ILogger<string> logger, string userId)
    {
        var request = await sender.Send(new GetUserRequest(userId));

        if (request.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно получен: {userId}");
            return TypedResults.Ok(request);
        }

        logger.LogError($"LogDebugError ================ Ошибка получения пользователя: {userId}");
        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<string>>, BadRequest<string>>> CreateUser(ISender sender,
        ILogger<CreateUserDto> logger, [FromBody] CreateUserDto createUserDto)
    {
        var command = await sender.Send(new CreateUserRequest(createUserDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно создан: {createUserDto.UserName}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Удалить пользователея не удалось: {createUserDto.UserName}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> UpdateUser(ISender sender, string userId,
        ILogger<UpdateUserDto> logger, [FromBody] UpdateUserDto updateUserDto)
    {
        var command = await sender.Send(new UpdateUserRequest(updateUserDto));

        if (command.IsSuccess && userId == updateUserDto.Id)
        {
            logger.LogDebug($"LogDebug ================ Пользователь успешно обновлен: {updateUserDto.Id}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Обновить пользователеля не удалось: {updateUserDto.Id}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> UpdateUserProfile(ISender sender,
        string userId,
        ILogger<UpdateUserProfileDto> logger, [FromForm] UpdateUserProfileDto updateUserProfileDto)
    {
        var command = await sender.Send(new UpdateUserProfileRequest() { UpdateUserProfile = updateUserProfileDto });

        if (command.IsSuccess && userId == updateUserProfileDto.Id)
        {
            logger.LogDebug(
                $"LogDebug ================ Профиль пользователя успешно обновлен: {updateUserProfileDto.Id}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Обновить профиль пользователеля не удалось: {updateUserProfileDto.Id}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> DeleteUser(ISender sender, string userId,
        ILogger<DeleteUserDto> logger, [FromBody] DeleteUserDto deleteUserDto)
    {
        var command = await sender.Send(new DeleteUserRequest(deleteUserDto));

        if (command.IsSuccess && userId == deleteUserDto.UserId)
        {
            logger.LogDebug($"LogDebug ================ Пользователя успешно удален: {deleteUserDto.UserId}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Удалить пользователеля не удалось: {deleteUserDto.UserId}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}