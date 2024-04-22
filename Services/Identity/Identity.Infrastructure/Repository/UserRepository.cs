using Identity.Domain.Entities.Users;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repository;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public sealed class UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    : IUserRepository
{
    public IQueryable<ApplicationUser> GetAll(CancellationToken cancellationToken = default)
    {
        return userManager.Users.AsNoTracking().AsQueryable();
    }

    public async Task<IEnumerable<IdentityRole<string>>> GetUserRolesAsync(ApplicationUser user,
        CancellationToken token = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Объект пустой");
        }

        var userRolesNames = await userManager.GetRolesAsync(user);
        var userRoles = userRolesNames.Select(roleName => new IdentityRole<string> { Name = roleName });
        return userRoles;
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, CancellationToken token = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Объект пустой");
        }

        await userManager.CreateAsync(user);
        await context.SaveChangesAsync(token);

        return await Task.FromResult(user);
    }

    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user, CancellationToken token = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Объект пустой");
        }

        await userManager.UpdateAsync(user);
        await context.SaveChangesAsync(token);

        return await Task.FromResult(user);
    }

    public async Task<ApplicationUser> DeleteUserAsync(ApplicationUser user, CancellationToken token = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Объект пустой");
        }

        await userManager.DeleteAsync(user);
        await context.SaveChangesAsync(token);

        return await Task.FromResult(user);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, CancellationToken token = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "Объект пустой");
        }

        var result = await userManager.CheckPasswordAsync(user, password);
        return result;
    }
}