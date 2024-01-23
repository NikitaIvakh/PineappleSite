using Newtonsoft.Json;
using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Response;
using ShoppingCart.Application.Services.IServices;

namespace ShoppingCart.Application.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Product");
            var response = await httpClient.GetAsync($"api/Product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responce = JsonConvert.DeserializeObject<ShoppingCartAPIResponse>(apiContent);

            if (responce.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responce.Data));
            }

            return new List<ProductDto>();
        }
    }
}