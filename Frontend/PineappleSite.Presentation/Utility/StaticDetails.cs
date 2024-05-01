using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Utility;

public static class StaticDetails
{
    public enum UserRoles
    {
        [Display(Name = "Пользователь")] User = 1,

        [Display(Name = "Администратор")] Administrator = 2
    }

    public const string RoleAdmin = "Administrator";
    public const string RoleUser = "User";
    public const string UserAdmin = "admin@localhost.com";

    public const string StatusPending = "Ожидается";
    public const string StatusApproved = "Подтвержден";
    public const string StatusReadyForPickup = "Готов к выдаче";
    public const string StatusReceivedByCourier = "Заказ получен курьером";
    public const string StatusBeingDelivered = "Заказ доставляется";
    public const string StatusDelivered = "Заказ доставлен";
    public const string StatusCompleted = "Завершен";
    public const string StatusRefunded = "Возвращен";
    public const string StatusCancelled = "Отменен";

    public static byte[] ConvertFormFileToByteArray(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}