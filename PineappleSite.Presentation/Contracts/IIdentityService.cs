using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IIdentityService
    {
        Task<IdentityResult<AuthResponseViewModel>> LoginAsync(AuthRequestViewModel authRequestViewModel);

        Task<IdentityResult<RegisterResponseViewModel>> RegisterAsync(RegisterRequestViewModel registerRequestViewModel);

        Task<IdentityResult<LogoutUserViewModel>> LogoutAsync(LogoutUserViewModel logoutUserViewModel);
    }
}