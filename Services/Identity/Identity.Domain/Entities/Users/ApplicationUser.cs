using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Description { get; set; }

        public int? Age { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiresTime { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}