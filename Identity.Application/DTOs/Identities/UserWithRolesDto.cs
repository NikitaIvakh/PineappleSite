using Identity.Core.Entities.User;

namespace Identity.Application.DTOs.Identities
{
    public class UserWithRolesDto
    {
        public ApplicationUser User { get; set; }

        public IList<string> Roles { get; set; }
    }
}