using Identity.Core.Entities.Identities;

namespace Identity.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest authRequest);

        Task<RegisterResponse> Register(RegisterRequest registerRequest);
    }
}