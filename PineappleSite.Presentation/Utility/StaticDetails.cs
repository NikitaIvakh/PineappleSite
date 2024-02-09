using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Utility
{
    public static class StaticDetails
    {
        public enum UserRoles
        {
            [Display(Name = "Пользователь")]
            Employee = 1,

            [Display(Name = "Администратор")]
            Administrator = 2
        }


        public const string RoleAdmin = "Administrator";
        public const string RoleEmployee = "Employee";
        public const string UserAdmin = "admin@localhost.com";
    }
}