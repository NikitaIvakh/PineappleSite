using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Entities.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Description { get; set; }

        public int? Age { get; set; }
    }
}