namespace Order.Domain.DTOs;

public sealed class OrderHeaderDto
{
    public int OrderHeaderId { get; set; }

    public string? UserId { get; init; }

    public string? CouponCode { get; init; }

    public double Discount { get; init; }

    public double OrderTotal { get; set; }


    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Address { get; init; }

    public DateTime? DeliveryDate { get; init; }


    public DateTime OrderTime { get; set; }

    public string? Status { get; set; }

    public string? PaymentIntentId { get; init; }

    public string? StripeSessionId { get; init; }

    public IEnumerable<OrderDetailsDto>? OrderDetails { get; set; }
}