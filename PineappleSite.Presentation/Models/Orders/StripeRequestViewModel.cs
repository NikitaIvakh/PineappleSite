namespace PineappleSite.Presentation.Models.Orders
{
    public class StripeRequestViewModel
    {
        public string? StripeSessionUrl { get; set; }

        public string? StripeSessionId { get; set; }

        public string ApprovedUrl { get; set; }

        public string CancelUrl { get; set; }

        public OrderHeaderViewModel OrderHeader { get; set; }
    }
}