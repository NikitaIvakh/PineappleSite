using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Interfaces;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetAll(CancellationToken token = default);

    Task<IEnumerable<string>> GetUserRolesAsync(ApplicationUser user, CancellationToken token = default);
    
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, CancellationToken token = default);

    Task<ApplicationUser> AddUserToRoleAsync(ApplicationUser user, string role, CancellationToken token = default);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken token = default);
    
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<IdentityResult> DeleteUserAsync(ApplicationUser user, CancellationToken token = default);
}