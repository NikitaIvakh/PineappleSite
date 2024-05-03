namespace PineappleSite.Presentation.Services.Orders;

public partial interface IOrderClient
{
    HttpClient HttpClient { get; }
}