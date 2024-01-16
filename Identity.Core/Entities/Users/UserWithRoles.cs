using Identity.Core.Entities.User;

namespace Identity.Core.Entities.Users
{
    public class UserWithRoles
    {
        public ApplicationUser User { get; set; }

        public IList<string> Roles { get; set; }
    }
}