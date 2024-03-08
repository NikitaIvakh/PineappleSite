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

        public const string Status_Pending = "Ожидается";
        public const string Status_Approved = "Подтвержден";
        public const string Status_ReadyForPickup = "Готов к выдаче";
        public const string Status_Completed = "Завершен";
        public const string Status_Refunded = "Возвращен";
        public const string Status_Cancelled = "Отменен";
    }
}