using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IUserService
    {
        Task<IList<UserWithRolesViewModel>> GetAllUsersAsync();

        Task<UserWithRolesViewModel> GetUserAsync(string id);

        Task<IdentityResponseViewModel> UpdateUserAsync(RegisterRequestViewModel register);

        Task<IdentityResponseViewModel> DeleteUserAsync(DeleteUserViewModel delete);
    }
}