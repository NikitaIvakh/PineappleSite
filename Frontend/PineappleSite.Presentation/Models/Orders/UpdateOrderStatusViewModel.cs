namespace PineappleSite.Presentation.Models.Orders;

public sealed class UpdateOrderStatusViewModel(int orderHeaderId, string newStatus)
{
    public int OrderHeaderId { get; set; } = orderHeaderId;

    public string NewStatus { get; set; } = newStatus;
}