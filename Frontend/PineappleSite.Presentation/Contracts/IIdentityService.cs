using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts;

public interface IIdentityService
{
    Task<IdentityResult<string>> LoginAsync(AuthRequestViewModel authRequestViewModel);

    Task<IdentityResult<string>> RegisterAsync(RegisterRequestViewModel registerRequestViewModel);

    Task<IdentityResult<ObjectResult>> RefreshTokenAsync(TokenModelViewModel tokenModelViewModel);

    Task<IdentityResult> RevokeTokenAsync(string userName);

    Task<IdentityResult> RevokeAllTokensAsync();
}