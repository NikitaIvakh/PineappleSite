namespace Order.Domain.Entities;

public class OrderHeader
{
    public int OrderHeaderId { get; init; }

    public string? UserId { get; init; }

    public string? CouponCode { get; init; }

    public double Discount { get; init; }

    public double OrderTotal { get; init; }


    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Address { get; init; }

    public DateTime? DeliveryDate { get; init; }


    public DateTime OrderTime { get; init; }

    public string? Status { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? StripeSessionId { get; set; }

    public IEnumerable<OrderDetails>? OrderDetails { get; init; }
}