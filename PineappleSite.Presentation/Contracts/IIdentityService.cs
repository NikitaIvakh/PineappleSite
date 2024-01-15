using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IIdentityService
    {
        Task<IdentityResponseViewModel> LoginAsync(AuthRequestViewModel authRequestViewModel);

        Task<IdentityResponseViewModel> RegisterAsync(RegisterRequestViewModel registerRequestViewModel);

        Task LogoutAsync();
    }
}