using Identity.Domain.Entities.Users;

namespace Identity.Domain.DTOs.Identities
{
    public class UserWithRolesDto
    {
        public ApplicationUser User { get; set; }

        public IList<string> Roles { get; set; }
    }
}