using PineappleSite.Presentation.Models.Users;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Contracts;

public interface IUserService
{
    Task<IdentityCollectionResult<GetUsersViewModel>> GetAllUsersAsync();
    
    Task<IdentityCollectionResult<GetUsersProfileViewModel>> GetUsersProfileAsync();

    Task<IdentityResult<GetUserViewModel>> GetUserAsync(string id);

    Task<IdentityResult<GetUserForUpdateViewModel>> GetUserAsync(string userId, string? password);

    Task<IdentityResult<string>> CreateUserAsync(CreateUserViewModel createUserViewModel);

    Task<IdentityResult> UpdateUserAsync(UpdateUserViewModel updateUserView);

    Task<IdentityResult<GetUserForUpdateViewModel>> UpdateUserProfileAsync(UpdateUserProfileViewModel updateUserProfile);

    Task<IdentityResult> DeleteUserAsync(DeleteUserViewModel delete);

    Task<IdentityResult> DeleteUsersAsync(DeleteUsersViewModel deleteUsers);
}