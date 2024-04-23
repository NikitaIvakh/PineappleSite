using Identity.Domain.Entities.Users;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Test.Common;

public static class IdentityDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        var hasher = new PasswordHasher<ApplicationUser>();
        context.AddRange(
            new ApplicationUser
            {
                Id = "3B189631-D179-4200-B77C-B8FC0FD28037",
                Email = "user_test_@localhost.com",
                NormalizedEmail = "USER_TEST_@LOCALHOST.COM",
                FirstName = "System",
                LastName = "User_Test_",
                UserName = "user_test_@localhost.com",
                NormalizedUserName = "USER_TEST_@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                EmailConfirmed = true,
                Description = "Test_Test",
                Age = 24,
            },

            new IdentityRole
            {
                Id = "C26B100F-216D-4DCD-8FEC-44439AF6C086",
                Name = "Employee",
                NormalizedName = "EMPLOYEE",
            },

            new IdentityUserRole<string>
            {
                RoleId = "C26B100F-216D-4DCD-8FEC-44439AF6C086",
                UserId = "3B189631-D179-4200-B77C-B8FC0FD28037"
            });

        context.SaveChanges();
        return context;
    }

    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}