namespace PineappleSite.Presentation.Models.Orders;

public sealed class StripeRequestViewModel
{
    public string? StripeSessionUrl { get; init; }

    public string? StripeSessionId { get; init; }

    public string? ApprovedUrl { get; init; }

    public string? CancelUrl { get; init; }

    public OrderHeaderViewModel? OrderHeader { get; init; }
}