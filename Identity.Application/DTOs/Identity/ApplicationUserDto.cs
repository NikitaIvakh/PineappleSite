using Microsoft.AspNetCore.Identity;

namespace Identity.Application.DTOs.Identity
{
    public class ApplicationUserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}