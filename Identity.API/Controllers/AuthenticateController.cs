using Identity.Application.DTOs.Authentications;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
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
        public async Task<ActionResult<BaseIdentityResponse<AuthResponseDto>>> Login([FromBody] AuthRequestDto authRequest)
        {
            var login = await _mediator.Send(new LoginUserRequest { AuthRequest = authRequest });
            return Ok(login);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseIdentityResponse<RegisterResponseDto>>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var register = await _mediator.Send(new RegisterUserRequest { RegisterRequest = registerRequest });
            return Ok(register);
        }
    }
}