using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Utilities
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

        public static string AdministratorRole = "Administrator";
        public static string EmployeeRole = "Employee";
    }
}