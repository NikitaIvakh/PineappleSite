using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Identity.Core.Entities.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}