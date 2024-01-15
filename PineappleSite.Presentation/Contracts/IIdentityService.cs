using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IIdentityService
    {
        Task<bool> LoginAsync(AuthRequestViewModel authRequestViewModel);

        Task<bool> RegisterAsync(RegisterRequestViewModel registerRequestViewModel);

        Task LogoutAsync();
    }
}