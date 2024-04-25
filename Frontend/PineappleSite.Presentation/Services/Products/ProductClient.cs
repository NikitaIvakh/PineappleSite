namespace PineappleSite.Presentation.Services.Products;

public partial class ProductClient : IProductClient
{
    public HttpClient HttpClient
    {
        get { return _httpClient; }
    }
}