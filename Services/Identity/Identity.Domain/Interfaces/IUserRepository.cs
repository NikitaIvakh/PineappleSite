using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Interfaces;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetAll(CancellationToken cancellationToken = default);

    Task<IEnumerable<IdentityRole<string>>> GetUserRolesAsync(ApplicationUser user, CancellationToken token = default);

    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, CancellationToken token = default);

    Task<ApplicationUser> AddUserToRoleAsync(ApplicationUser user, string role, CancellationToken token = default);

    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<IdentityResult> DeleteUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken token = default);
}