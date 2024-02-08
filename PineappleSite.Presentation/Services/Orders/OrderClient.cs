namespace PineappleSite.Presentation.Services.Orders
{
    public partial class OrderClient : IOrderClient
    {
        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }
    }
}