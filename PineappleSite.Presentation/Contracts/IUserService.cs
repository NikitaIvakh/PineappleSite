using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IUserService
    {
        Task<IList<UserWithRolesViewModel>> GetAllUsersAsync();

        Task<IdentityResponseViewModel> GetUsersAsync();

        Task<IdentityResponseViewModel> UpdateUserAsync(RegisterRequestViewModel register);

        Task<IdentityResponseViewModel> DeleteUserAsync(DeleteUserViewModel delete);
    }
}