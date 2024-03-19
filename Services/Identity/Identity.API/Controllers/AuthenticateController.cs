using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Domain.ResultIdentity;
using Identity.Domain.DTOs.Authentications;
using Identity.Application.Features.Identities.Requests.Commands;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController(IMediator mediator, ILogger<AuthResponseDto> authLogger, ILogger<RegisterResponseDto> registerLogger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<AuthResponseDto> _authLogger = authLogger;
        private readonly ILogger<RegisterResponseDto> _registerLogger = registerLogger;

        [HttpPost("Login")]
        public async Task<ActionResult<Result<AuthResponseDto>>> Login([FromBody] AuthRequestDto authRequest)
        {
            var login = await _mediator.Send(new LoginUserRequest { AuthRequest = authRequest });

            if (login.IsSuccess)
            {
                _authLogger.LogDebug($"LogDebug ================ Уcпешный вход в аккаунт: {authRequest.Email}");
                return Ok(login);
            }

            _authLogger.LogError($"LogDebugError ================ Войти в аккаунт не удалось: {authRequest.Email}");
            foreach (var error in login.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var register = await _mediator.Send(new RegisterUserRequest { RegisterRequest = registerRequest });

            if (register.IsSuccess)
            {
                _registerLogger.LogDebug($"LogDebug ================ Уcпешная регистрация: {registerRequest.Email}");
                return Ok(register);
            }

            _registerLogger.LogError($"LogDebugError ================ Регистрация не удалась: {registerRequest.Email}");
            foreach (var error in register.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<Result<ObjectResult>>> RefreshToken([FromBody] TokenModelDto tokenModel)
        {
            var command = await _mediator.Send(new RefreshTokenRequest { TokenModelDto = tokenModel });

            if (command.IsSuccess)
            {
                _authLogger.LogDebug($"LogDebug ================ Токен успешно обновлен: {tokenModel}");
                return Ok(command);
            }

            _registerLogger.LogError($"LogDebugError ================ Обновить токен не удалось: {tokenModel}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpPost("RevokeToken/{userName}")]
        public async Task<ActionResult<Result<Unit>>> RevokeToken(string userName)
        {
            var command = await _mediator.Send(new RevorkeTokenRequest { UserName = userName });

            if (command.IsSuccess)
            {
                _authLogger.LogDebug($"LogDebug ================ Токен успешно удален: {userName}");
                return Ok(command);
            }

            _registerLogger.LogError($"LogDebugError ================ Удалить токен не удалось: {userName}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpPost("RevokeAllTokens")]
        public async Task<ActionResult<Result<Unit>>> RevokeAllTokens()
        {
            var command = await _mediator.Send(new RevokeAllTokensRequest());

            if (command.IsSuccess)
            {
                _authLogger.LogDebug($"LogDebug ================ Токены успешно удалены");
                return Ok(command);
            }

            _authLogger.LogError($"LogDebugError ================ Токены не могут быть удалены");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}