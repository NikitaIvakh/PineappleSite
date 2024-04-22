using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Interfaces;

public interface IUserRepository
{
    IQueryable<ApplicationUser> GetAll(CancellationToken cancellationToken = default);

    Task<IEnumerable<IdentityRole<string>>> GetUserRolesAsync(ApplicationUser user, CancellationToken token = default);
    
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<ApplicationUser> DeleteUserAsync(ApplicationUser user, CancellationToken token = default);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken token = default);
}