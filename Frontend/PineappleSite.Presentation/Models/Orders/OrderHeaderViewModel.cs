using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Orders;

public sealed class OrderHeaderViewModel
{
    public int OrderHeaderId { get; init; }

    public string? UserId { get; init; }

    public string? CouponCode { get; init; }

    public double Discount { get; init; }

    public double OrderTotal { get; init; }


    [Required(ErrorMessage = "Поле с именем не может быть пустым")]
    [MaxLength(45, ErrorMessage = "Поле не может превышать 45 символов")]
    [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
    public string? Name { get; init; }

    [DataType(DataType.EmailAddress, ErrorMessage = "Ведите действительный адрес электронной почты")]
    [EmailAddress(ErrorMessage = "Почтовый адрес не может быть пустым")]
    [Required(ErrorMessage = "Поле с почтовым адресом не может быть пустым")]
    [MaxLength(45, ErrorMessage = "Почтовый адрес не должен превышать 45 символов")]
    [MinLength(2, ErrorMessage = "Почтовый адрес должен быть более 2 символов")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Поле с номером телефона не может быть пустым")]
    [MaxLength(12, ErrorMessage = "Номер телефона не должен превышать 12 символов")]
    [MinLength(7, ErrorMessage = "Номер телефона должен быть более 7 символов")]
    public string? PhoneNumber { get; init; }

    [Required(ErrorMessage = "Адрес доставки не может быть пустым")]
    [MinLength(2, ErrorMessage = "Адрес доставки должен быть более 2 символов")]
    [MaxLength(250, ErrorMessage = "Адрес доставки не должен превышать 250 символов")]
    public string? Address { get; init; }

    public DateTime? DeliveryDate { get; init; }


    public DateTime OrderTime { get; init; }

    public string? Status { get; init; }

    public string? PaymentIntentId { get; init; }

    public string? StripeSessionId { get; init; }

    public IEnumerable<OrderDetailsViewModel>? OrderDetails { get; init; }
}