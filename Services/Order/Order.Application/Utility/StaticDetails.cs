namespace Order.Application.Utility
{
    public class StaticDetails
    {
        public const string Status_Pending = "Ожидается";
        public const string Status_Approved = "Подтвержден";
        public const string Status_ReadyForPickup = "Готов к выдаче";
        public const string Status_Completed = "Завершен";
        public const string Status_Refunded = "Возвращен";
        public const string Status_Cancelled = "Отменен";

        public const string RoleAdmin = "Administrator";
        public const string RoleUser = "User";
        public const string UserAdmin = "admin@localhost.com";
    }
}