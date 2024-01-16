using Microsoft.AspNetCore.Identity;

namespace Identity.Core.Entities.Users
{
    public class CreateUserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}