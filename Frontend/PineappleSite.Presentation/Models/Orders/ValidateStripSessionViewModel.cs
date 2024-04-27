namespace PineappleSite.Presentation.Models.Orders;

public class ValidateStripSessionViewModel(int orderHeaderId)
{
    public int OrderHeaderId { get; init; } = orderHeaderId;
}