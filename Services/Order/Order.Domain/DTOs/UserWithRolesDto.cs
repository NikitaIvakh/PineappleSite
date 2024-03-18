namespace Order.Domain.DTOs
{
    public class UserWithRolesDto
    {
        public ApplicationUserDto User { get; set; }

        public IList<string> Roles { get; set; }
    }
}