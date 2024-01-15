using Identity.Core.Entities.Identities;
using Identity.Core.Response;

namespace Identity.Core.Interfaces
{
    public interface IAuthService
    {
        Task<BaseIdentityResponse<AuthResponse>> LoginAsync(AuthRequest authRequest);

        Task<BaseIdentityResponse<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest);
    }
}