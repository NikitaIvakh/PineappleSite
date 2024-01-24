using ShoppingCart.Application.DTOs.Cart;
using ShoppingCart.Application.Services.IServices;
using System.Text.Json;

namespace ShoppingCart.Application.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/Product");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                var options = jsonSerializerOptions;
                return await JsonSerializer.DeserializeAsync<IReadOnlyCollection<ProductDto>>(stream, options);
            }

            return new List<ProductDto>();
        }
    }
}