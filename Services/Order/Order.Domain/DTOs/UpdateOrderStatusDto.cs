namespace Order.Domain.DTOs;

public sealed class UpdateOrderStatusDto(int orderHeaderId, string newStatus)
{
    public int OrderHeaderId { get; set; } = orderHeaderId;

    public string NewStatus { get; set; } = newStatus;
}