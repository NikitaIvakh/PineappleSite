namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public partial class ShoppingCartClient : IShoppingCartClient
    {
        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }
    }
}