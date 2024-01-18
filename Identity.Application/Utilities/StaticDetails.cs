using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Utilities
{
    public static class StaticDetails
    {
        public enum UserRoles
        {
            Employee = 1,
            Administrator = 2
        }

        public static string AdministratorRole = "Administrator";
        public static string EmployeeRole = "Employee";
    }
}