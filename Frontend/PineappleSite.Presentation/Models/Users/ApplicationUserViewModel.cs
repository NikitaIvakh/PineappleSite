using static PineappleSite.Presentation.Utility.StaticDetails;

namespace PineappleSite.Presentation.Models.Users
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Description { get; set; }

        public int? Age { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiresTime { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public UserRoles UserRoles { get; set; }
    }
}