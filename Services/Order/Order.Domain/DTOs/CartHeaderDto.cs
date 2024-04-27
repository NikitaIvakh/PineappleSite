namespace Order.Domain.DTOs;

public sealed class CartHeaderDto
{
    public int CartHeaderId { get; init; }

    public string? UserId { get; init; }

    public string? CouponCode { get; init; }

    public double Discount { get; init; }

    public double CartTotal { get; init; }


    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Address { get; init; }

    public DateTime? DeliveryDate { get; init; }
}