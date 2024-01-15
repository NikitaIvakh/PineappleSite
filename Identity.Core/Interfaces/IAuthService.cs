using Identity.Core.Entities.Identities;
using Identity.Core.Response;

namespace Identity.Core.Interfaces
{
    public interface IAuthService
    {
        Task<BaseIdentityResponse> LoginAsync(AuthRequest authRequest);

        Task<BaseIdentityResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}