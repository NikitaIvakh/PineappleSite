using Identity.Core.Entities.Identities;
using Identity.Core.Response;

namespace Identity.Core.Interfaces
{
    public interface IAuthService
    {
        Task<BaseIdentityResponse> Login(AuthRequest authRequest);

        Task<BaseIdentityResponse> Register(RegisterRequest registerRequest);
    }
}