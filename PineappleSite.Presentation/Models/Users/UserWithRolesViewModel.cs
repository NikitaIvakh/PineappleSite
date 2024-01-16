namespace PineappleSite.Presentation.Models.Users
{
    public class UserWithRolesViewModel
    {
        public ApplicationUserViewModel User { get; set; }

        public IList<string> Roles { get; set; }

        public string SelectedRole { get; set; }
    }
}