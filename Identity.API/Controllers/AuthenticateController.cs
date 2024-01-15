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
        public async Task<ActionResult<BaseIdentityResponse>> Login(AuthRequest authRequest)
        {
            return Ok(await _authService.LoginAsync(authRequest));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<BaseIdentityResponse>> Register(RegisterRequest registerRequest)
        {
            return Ok(await _authService.RegisterAsync(registerRequest));
        }
    }
}