using Identity.Domain.Enum;

namespace Identity.Domain.DTOs.Identities
{
    public class UpdateUserDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserName { get; set; }

        public UserRoles UserRoles { get; set; }
    }
}