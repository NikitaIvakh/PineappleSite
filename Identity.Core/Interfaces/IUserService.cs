using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Response;

namespace Identity.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser> GetByIdAsync(Guid id);

        Task<BaseIdentityResponse<ApplicationUser>> CreateUserAsync(RegisterRequest user);

        Task<BaseIdentityResponse<ApplicationUser>> UpdateUserAsync(RegisterRequest user);

        Task<BaseIdentityResponse<ApplicationUser>> DeleteUserAsync(Guid id);
    }
}