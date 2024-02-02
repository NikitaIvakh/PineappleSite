using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Domain.ResultIdentity;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.DTOs.Authentications;
using Identity.Application.Features.Identities.Requests.Commands;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Login")]
        public async Task<ActionResult<Result<AuthResponseDto>>> Login([FromBody] AuthRequestDto authRequest)
        {
            var login = await _mediator.Send(new LoginUserRequest { AuthRequest = authRequest });

            if (login.IsSuccess)
            {
                return Ok(login);
            }

            return BadRequest(login.ErrorMessage);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var register = await _mediator.Send(new RegisterUserRequest { RegisterRequest = registerRequest });

            if (register.IsSuccess)
            {
                return Ok(register);
            }

            return BadRequest(register.ValidationErrors);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<Result<bool>>> Logout()
        {
            var command = await _mediator.Send(new LogoutUserRequest());

            if (command.IsSuccess)
            {
                return Ok(command);
            }

            return BadRequest(command.ErrorMessage);
        }
    }
}