using Carter;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Endpoints;

public sealed class AuthenticateEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authenticate");

        group.MapPost("/Login", Login).WithName(nameof(Login));
        group.MapPost("/Register", Register).WithName(nameof(Register));
        group.MapPost("/RefreshToken", RefreshToken).WithName(nameof(RefreshToken));
        group.MapPost("/RevokeToken/{userName}", RevokeToken).WithName(nameof(RevokeToken));
        group.MapPost("/RevokeTokens", RevokeTokens).WithName(nameof(RevokeTokens));
    }

    private static async Task<Results<Ok<Result<AuthResponseDto>>, BadRequest<string>>> Login(ISender sender,
        ILogger<AuthRequestDto> logger, [FromBody] AuthRequestDto authRequestDto)
    {
        var command = await sender.Send(new LoginUserRequest(authRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Уcпешный вход в аккаунт: {authRequestDto.EmailAddress}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка входа в аккаунт: {authRequestDto.EmailAddress}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<string>>, BadRequest<string>>> Register(ISender sender,
        ILogger<RegisterUserRequest> logger, [FromBody] RegisterRequestDto registerRequestDto)
    {
        var command = await sender.Send(new RegisterUserRequest(registerRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Уcпешная регистрация: {registerRequestDto.EmailAddress}");
            return TypedResults.Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Регистрация не удалась: {registerRequestDto.EmailAddress}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<ObjectResult>>, BadRequest<string>>> RefreshToken(ISender sender,
        ILogger<TokenModelDto> logger, [FromBody] TokenModelDto tokenModelDto)
    {
        var command = await sender.Send(new RefreshTokenRequest(tokenModelDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токен успешно обновлен: {tokenModelDto}");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Обновить токен не удалось: {tokenModelDto}");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    private static async Task<Results<Ok<Result<Unit>>, BadRequest<string>>> RevokeToken(ISender sender,
        ILogger<string> logger, [FromRoute] string userName)
    {
        var commnad = await sender.Send(new RevokeTokenRequest(userName));

        if (commnad.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токен успешно удален: {userName}");
            return TypedResults.Ok(commnad);
        }

        logger.LogError($"LogDebugError ================ Удалить токен не удалось: {userName}");
        return TypedResults.BadRequest(string.Join(", ", commnad.ValidationErrors!));
    }

    private static async Task<Results<Ok<CollectionResult<Unit>>, BadRequest<string>>> RevokeTokens(ISender sender, ILogger<Unit> logger)
    {
        var command = await sender.Send(new RevokeTokensRequest());

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токены успешно удалены");
            return TypedResults.Ok(command);
        }

        logger.LogError($"LogDebugError ================ Токены не могут быть удалены");
        return TypedResults.BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}