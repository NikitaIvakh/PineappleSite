namespace Order.Domain.DTOs;

public sealed class ValidateStripeSessionDto(int orderHeaderId)
{
    public int OrderHeaderId { get; set; } = orderHeaderId;
}