using Microsoft.AspNetCore.Identity;

namespace Identity.Application.DTOs.Identities
{
    public class CreateUserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}