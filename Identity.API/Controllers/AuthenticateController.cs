using Identity.Core.Entities.Identities;
using Identity.Core.Interfaces;
using Identity.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]
        public async Task<ActionResult<BaseIdentityResponse<AuthResponse>>> Login([FromBody] AuthRequest authRequest)
        {
            var login = await _authService.LoginAsync(authRequest);
            return Ok(login);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseIdentityResponse<RegisterResponse>>> Register([FromBody] RegisterRequest registerRequest)
        {
            var register = await _authService.RegisterAsync(registerRequest);
            return Ok(register);
        }
    }
}