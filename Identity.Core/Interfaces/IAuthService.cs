using Identity.Core.Entities.Identities;

namespace Identity.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(AuthRequest authRequest);

        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}