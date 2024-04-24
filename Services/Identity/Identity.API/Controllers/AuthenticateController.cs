using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticateController : ControllerBase
{
    [HttpPost("Login")]
    public async Task<ActionResult<Result<string>>> Login(ISender sender, ILogger<AuthRequestDto> logger,
        [FromBody] AuthRequestDto authRequestDto)
    {
        var command = await sender.Send(new LoginUserRequest(authRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Уcпешный вход в аккаунт: {authRequestDto.EmailAddress}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Ошибка входа в аккаунт: {authRequestDto.EmailAddress}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("Register")]
    public async Task<ActionResult<Result<string>>> Register(ISender sender, ILogger<RegisterUserRequest> logger,
        [FromBody] RegisterRequestDto registerRequestDto)
    {
        var command = await sender.Send(new RegisterUserRequest(registerRequestDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Уcпешная регистрация: {registerRequestDto.EmailAddress}");
            return Ok(command);
        }

        logger.LogError(
            $"LogDebugError ================ Регистрация не удалась: {registerRequestDto.EmailAddress}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<Result<ObjectResult>>> RefreshToken(ISender sender,
        ILogger<TokenModelDto> logger, [FromBody] TokenModelDto tokenModelDto)
    {
        var command = await sender.Send(new RefreshTokenRequest(tokenModelDto));

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токен успешно обновлен: {tokenModelDto}");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Обновить токен не удалось: {tokenModelDto}");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }

    [HttpPost("RevokeToken")]
    public async Task<ActionResult<Result<Unit>>> RevokeToken(ISender sender, ILogger<string> logger,
        [FromRoute] string userName)
    {
        var commnad = await sender.Send(new RevokeTokenRequest(userName));

        if (commnad.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токен успешно удален: {userName}");
            return Ok(commnad);
        }

        logger.LogError($"LogDebugError ================ Удалить токен не удалось: {userName}");
        return BadRequest(string.Join(", ", commnad.ValidationErrors!));
    }

    [HttpPost("RevokeTokens")]
    public async Task<ActionResult<CollectionResult<Unit>>> RevokeTokens(ISender sender, ILogger<Unit> logger)
    {
        var command = await sender.Send(new RevokeTokensRequest());

        if (command.IsSuccess)
        {
            logger.LogDebug($"LogDebug ================ Токены успешно удалены");
            return Ok(command);
        }

        logger.LogError($"LogDebugError ================ Токены не могут быть удалены");
        return BadRequest(string.Join(", ", command.ValidationErrors!));
    }
}