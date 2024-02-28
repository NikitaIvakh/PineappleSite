using Identity.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.DTOs.Identities
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public UserRoles Roles { get; set; }
    }
}