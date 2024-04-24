using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Interfaces;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetUsers();
    
    Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user);
    
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, CancellationToken token = default);

    Task<IdentityResult> RemoveFromRolesAsync(ApplicationUser user, CancellationToken token = default);

    Task<ApplicationUser> AddUserToRoleAsync(ApplicationUser user, string role, CancellationToken token = default);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
}