namespace Order.Application.Utility;

public static class StaticDetails
{
    public const string StatusPending = "Ожидается";
    public const string StatusApproved = "Подтвержден";
    public const string StatusReadyForPickup = "Готов к выдаче";
    public const string StatusCompleted = "Завершен";
    public const string StatusRefunded = "Возвращен";
    public const string StatusCancelled = "Отменен";

    public const string RoleAdmin = "Administrator";
    public const string RoleUser = "User";
    public const string UserAdmin = "admin@localhost.com";
}