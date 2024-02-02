using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts
{
    public interface IUserService
    {
        Task<IdentityCollectionResult<UserWithRolesViewModel>> GetAllUsersAsync(string userId);

        Task<UserWithRolesViewModel> GetUserAsync(string id);

        Task<IdentityResult<RegisterResponseViewModel>> CreateUserAsync(RegisterRequestViewModel register);

        Task<IdentityResult<RegisterResponseViewModel>> UpdateUserAsync(UpdateUserViewModel updateUserView);

        Task<IdentityResult<UserWithRolesViewModel>> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfile);

        Task<IdentityResult<DeleteUserViewModel>> DeleteUserAsync(DeleteUserViewModel delete);

        Task<IdentityResult<bool>> DeleteUsersAsync(DeleteUserListViewModel deleteUsers);
    }
}