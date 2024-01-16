using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using Identity.Core.Response;

namespace Identity.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserWithRoles>> GetAllUsersAsync();

        Task<UserWithRoles> GetByIdAsync(string id);

        Task<BaseIdentityResponse<ApplicationUser>> CreateUserAsync(RegisterRequest user);

        Task<BaseIdentityResponse<ApplicationUser>> UpdateUserAsync(RegisterRequest user);

        Task<BaseIdentityResponse<ApplicationUser>> DeleteUserAsync(Guid id);
    }
}