using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentTime = DateTime.UtcNow;

        foreach (var entity in ChangeTracker.Entries<ApplicationUser>()
                     .Where(key => key.State is EntityState.Added or EntityState.Modified))
        {
            entity.Entity.ModifiedTime = currentTime;

            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedTime = currentTime;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}