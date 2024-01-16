using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Entities.User
{
    public class ApplicationUserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}