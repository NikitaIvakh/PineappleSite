using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(login);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var register = await _mediator.Send(new RegisterUserRequest { RegisterRequest = registerRequest });
            return Ok(register);
        }

        [HttpPost("Logout/{userId}")]
        public async Task<ActionResult<Result<LogoutUserDto>>> Logout([FromBody] LogoutUserDto logoutUser)
        {
            var command = await _mediator.Send(new LogoutUserRequest { LogoutUser = logoutUser });
            return Ok(command);
        }
    }
}