using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class ApplicationUserDto : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}